namespace CardManagement.Services.Interfaces
{
    public interface ICommonService
    {
        Task<string> GetExtention(byte[] document);
        Task<string> UploadAttachmentAsync(string filename, byte[] bytesToWrite);
        Task<string> AddFiles(string base64File);
        Task<string> GetMimeTypeFromBase64(string base64File);
        Task<string> GenerateFileNameFromMimeType(string mimeType);
        string GenerateCVV();
        string GenerateCardNumber();
        string GeneratePin();
    }
}
