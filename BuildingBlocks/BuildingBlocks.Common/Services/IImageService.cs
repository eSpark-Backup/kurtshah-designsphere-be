namespace BuildingBlocks.Common.Services;

public interface IImageService
{
    string UploadImage(Stream data, string destination, string module);
}
