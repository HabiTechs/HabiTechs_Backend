using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HabiTechs.Services; 

public class AzureBlobService
{
    private readonly BlobContainerClient? _container;
    private readonly bool _isMockMode; // Bandera para saber si estamos simulando

    public AzureBlobService(IConfiguration config)
    {
        var conn = config["AzureBlob:ConnectionString"];
        var containerName = config["AzureBlob:Container"] ?? "residents";
        
        // Si dice "MOCK_MODE" o está vacío, activamos el modo simulación
        if (string.IsNullOrEmpty(conn) || conn == "MOCK_MODE" || conn.Contains("TU_ACCOUNT")) 
        {
             _isMockMode = true;
             return; // No intentamos conectar a Azure para que no explote
        }

        try 
        {
            var client = new BlobServiceClient(conn);
            _container = client.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists();
            _container.SetAccessPolicy(PublicAccessType.Blob);
            _isMockMode = false;
        }
        catch
        {
            // Si falla la conexión (ej. internet), pasamos a modo simulación
            _isMockMode = true; 
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file, string prefix = "img")
    {
        if (file == null || file.Length == 0) return "";

        // --- MODO SIMULACIÓN ---
        if (_isMockMode || _container == null)
        {
            // Devolvemos una imagen de internet cualquiera para que el frontend funcione
            return $"https://via.placeholder.com/150?text={prefix}_MockImage";
        }
        
        // --- MODO REAL (AZURE) ---
        var ext = Path.GetExtension(file.FileName);
        var blobName = $"{prefix}_{Guid.NewGuid():N}{ext}";
        var blobClient = _container.GetBlobClient(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();
    }
}