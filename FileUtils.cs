using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace Software.Boll.EasyUtils;

#region Result Structs

public readonly record struct FileResult(
    bool Success = false,
    long CharsWritten = 0,
    long BytesWritten = 0,
    long CharsRead = 0,
    long BytesRead = 0,
    string FileName = "",
    string FilePath = "",
    bool FileExists = false,
    string? Text = "",
    uint[]? Bytes = null,
    Exception? Exception = null
);

#endregion

public static class FileUtils
{
    #region Text

    public static FileResult WriteText(string filePath, string contents)
    {
        var fileName = Path.GetFileName(filePath);
        try
        {
            var dirRes = EnsureDirectoryExists(filePath);
            if (!dirRes.Success) return dirRes;
            File.WriteAllText(filePath, contents);
            return new FileResult { Success = true, CharsWritten = contents.Length, FileName = fileName, FilePath = filePath };
        }
        catch (Exception ex)
        {
            return new FileResult { FileName = fileName, FilePath = filePath, Exception = ex };
        }
    }

    public static FileResult ReadText(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        try
        {
            var fileRes = FileExists(filePath);
            if (!fileRes.Success) return fileRes;
            var text = File.ReadAllText(filePath);
            return new FileResult { Success = true, CharsRead = text.Length, FileName = fileName, FilePath = filePath, Text = text };
        }
        catch (Exception ex)
        {
            return new FileResult { FileName = fileName, FilePath = filePath, Exception = ex };
        }
    }

    #endregion

    #region Binary

    public static FileResult WriteBinary(string filePath, uint[] data)
    {
        // TODO: Implement
        // Make sure the folder structure to the file exists
        // Write binary to the file, overwrite if it exists.
        // Return true if the writing was successful.
        return new FileResult(false);
    }

    public static FileResult ReadBinary(string filePath)
    {
        // TODO: Implement
        return new FileResult(false);
    }

    #endregion

    #region Generic

    public static FileResult Delete(string filePath)
    {
        // TODO: Implement
        // Check so folder path to file exists
        // Try to delete file if it exists, return true if the file does not exist.
        return new FileResult(false);
    }

    #endregion

    #region Auxilliary

    private static FileResult FileExists(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        try
        {
            var exists = File.Exists(filePath);
            return new FileResult { Success = true, FileExists = exists, FileName = fileName, FilePath = filePath };
        }
        catch (Exception ex)
        {
            return new FileResult { Exception = ex };
        }
    }

    private static FileResult EnsureDirectoryExists(string filePath)
    {
        var path = Path.GetDirectoryName(filePath) ?? "";
        try
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return new FileResult { Success = true, FileExists = true, FilePath = path };
        }
        catch (Exception ex)
        {
            return new FileResult { FilePath = path, Exception = ex };
        }
    }

    #endregion
}