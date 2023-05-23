using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Horizon.Database.DTO;
using Horizon.Database.Entities;
using Horizon.Database.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using FileAttributes = Horizon.Database.Entities.FileAttributes;

namespace Horizon.Database.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileServicesController : ControllerBase
    {
        private Ratchet_DeadlockedContext db;
        public FileServicesController(Ratchet_DeadlockedContext _db)
        {
            db = _db;
        }

        #region CreateFile
        [Authorize("database")]
        [HttpPost, Route("createFile")]
        public async Task<dynamic> createFile(FileDTO fileReq)
        {
            Files existingFile = db.Files.Where(af => af.FileName == fileReq.FileName && af.OwnerID == fileReq.OwnerID && af.AppId == fileReq.AppId).FirstOrDefault();
            FileAttributes existingFileAttributes = db.FileAttributes.Where(af => af.AppId == fileReq.AppId && af.Description == fileReq.fileAttributesDTO.Description).FirstOrDefault();

            if (existingFile != null || existingFileAttributes != null)
                return StatusCode(403, "File already exists.");

            Files newFile = new Files()
            {
                AppId = fileReq.AppId,
                FileName = fileReq.FileName,
                ServerChecksum = fileReq.ServerChecksum,
                FileID = fileReq.FileID,
                FileSize = fileReq.FileSize,
                CreationTimeStamp = fileReq.CreationTimeStamp,
                OwnerID = fileReq.OwnerID,
                GroupID = fileReq.GroupID,
                OwnerPermissionRWX = fileReq.OwnerPermissionRWX,
                GroupPermissionRWX = fileReq.GroupPermissionRWX,
                GlobalPermissionRWX = fileReq.GlobalPermissionRWX,
                ServerOperationID = fileReq.ServerOperationID,
                CreateDt = DateTime.UtcNow
            };

            db.Files.Add(newFile);

            if (fileReq.fileAttributesDTO.Description != null)
            {
                FileAttributes newFileAttributes = new FileAttributes()
                {
                    FileID = fileReq.FileID,
                    Description = fileReq.fileAttributesDTO.Description,
                    LastChangedTimeStamp = fileReq.fileAttributesDTO.LastChangedTimeStamp,
                    LastChangedByUserID = fileReq.fileAttributesDTO.LastChangedByUserID,
                    NumberAccesses = fileReq.fileAttributesDTO.NumberAccesses,
                    StreamableFlag = fileReq.fileAttributesDTO.StreamableFlag,
                    StreamingDataRate = fileReq.fileAttributesDTO.StreamingDataRate,
                    CreateDt = DateTime.UtcNow
                };

                db.FileAttributes.Add(newFileAttributes);
            }

            db.SaveChanges();
            return Ok("File Added");
        }
        #endregion

        #region AddFile
        [Authorize("database")]
        [HttpPost, Route("addFile")]
        public async Task<dynamic> addFile([FromBody] FileDTO fileReq)
        {
            Files existingFile = db.Files.Where(af => af.AppId == fileReq.AppId 
                && af.FileName == fileReq.FileName).FirstOrDefault();
            FileAttributes existingFileAttributes = db.FileAttributes.Where(af => af.AppId == fileReq.AppId 
                && af.FileName == fileReq.FileName
                && af.Description == af.Description).FirstOrDefault();

            if (existingFile == null && existingFileAttributes == null)
            {
                Files newFile = new Files()
                {
                    AppId = fileReq.AppId,
                    FileName = fileReq.FileName,
                    ServerChecksum = fileReq.ServerChecksum,
                    FileID = fileReq.FileID,
                    FileSize = fileReq.FileSize,
                    CreationTimeStamp = fileReq.CreationTimeStamp,
                    OwnerID = fileReq.OwnerID,
                    GroupID = fileReq.GroupID,
                    OwnerPermissionRWX = fileReq.OwnerPermissionRWX,
                    GroupPermissionRWX = fileReq.GroupPermissionRWX,
                    GlobalPermissionRWX = fileReq.GlobalPermissionRWX,
                    ServerOperationID = fileReq.ServerOperationID,
                    CreateDt = DateTime.UtcNow,
                    ModifiedDt = DateTime.UtcNow,
                };

                db.Entry(newFile).State = EntityState.Added;
                db.Files.Add(newFile);

                if (fileReq.fileAttributesDTO.Description != null)
                {
                    FileAttributes newFileAttributes = new FileAttributes()
                    {
                        AppId = fileReq.AppId,
                        FileID = fileReq.FileID,
                        FileName = fileReq.FileName,
                        Description = fileReq.fileAttributesDTO.Description,
                        LastChangedTimeStamp = fileReq.fileAttributesDTO.LastChangedTimeStamp,
                        LastChangedByUserID = fileReq.fileAttributesDTO.LastChangedByUserID,
                        NumberAccesses = fileReq.fileAttributesDTO.NumberAccesses,
                        StreamableFlag = fileReq.fileAttributesDTO.StreamableFlag,
                        StreamingDataRate = fileReq.fileAttributesDTO.StreamingDataRate,
                        CreateDt = DateTime.UtcNow,
                        ModifiedDt = DateTime.UtcNow,
                    };

                    db.Entry(newFileAttributes).State = EntityState.Added;
                    db.FileAttributes.Add(newFileAttributes);
                }

                db.SaveChanges();
                return Ok("File Added");
            }
            else
            {
                return StatusCode(403, "File or File Attributes already exists.");
            }
        }
        #endregion

        #region DeleteFile
        [Authorize("database")]
        [HttpPost, Route("deleteFile")]
        public async Task<dynamic> deleteFile([FromBody] FileDTO fileReq)
        {
            Files existingFile = db.Files.Where(af => af.FileName == fileReq.FileName).FirstOrDefault();

            FileAttributes existingFileAttributes = db.FileAttributes.Where(af => /*af.AppId == fileReq.AppId*/
                 af.FileName == fileReq.FileName).FirstOrDefault();

            if (existingFile != null)
            {
                db.Files.Remove(existingFile);
                db.Entry(existingFile).State = EntityState.Deleted;

                if(existingFileAttributes != null)
                {
                    db.FileAttributes.Remove(existingFileAttributes);
                    db.Entry(existingFileAttributes).State = EntityState.Deleted;
                }

                db.SaveChanges();
                return Ok("File Deleted");
            } else {

                return StatusCode(403, "Can't delete a file or file attributes that doesn't exist.");
            }



        }
        #endregion

        #region getFileListExt
        [Authorize("database")]
        [HttpGet, Route("getFileListExt")]
        public async Task<List<FileDTO>> getFileListExt(int AppId, string FileNameBeginsWith, int OwnerByID, string metaKey)
        {
            List<FileDTO> filesWithMetaDataReturn = new List<FileDTO>();
            
            if (OwnerByID < 0)
            {
                Console.WriteLine($"OwnerById < 0");

                Files filesListReturned = null;
                FileAttributes fileAttributes = null;
                FileMetaData fileMetaData = null;

                if (FileNameBeginsWith.Contains("*"))
                {
                    Console.WriteLine($"1 FileNameBeginsWith: {FileNameBeginsWith}");
                    filesListReturned = db.Files.Where(File => File.AppId == AppId)
                        .FirstOrDefault();

                    fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId)
                        .FirstOrDefault();

                    fileMetaData = db.FileMetaDatas.Where(x => x.AppId == AppId)
                        .FirstOrDefault();
                } else
                {
                    Console.WriteLine($"2 FileNameBeginsWith: {FileNameBeginsWith}");
                    filesListReturned = db.Files.Where(File => File.AppId == AppId &&
                        File.FileName.StartsWith(FileNameBeginsWith))
                        .FirstOrDefault();

                    fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId
                        && x.FileName.StartsWith(FileNameBeginsWith))
                        .FirstOrDefault();

                    fileMetaData = db.FileMetaDatas.Where(x => x.AppId == AppId
                        && x.FileName.StartsWith(FileNameBeginsWith)
                        && x.Key == metaKey)
                        .FirstOrDefault();
                }

                if (filesListReturned == null) {
                    Console.WriteLine("No Files found to list");
                    return null;
                } else if (fileAttributes == null) {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                } else if (fileMetaData == null) {
                    Console.WriteLine($"No File with key {metaKey} found to list");
                    return null;
                }

                FileDTO fileToList = new FileDTO()
                {
                    AppId = AppId,
                    FileID = filesListReturned.FileID,
                    ServerChecksum = filesListReturned.ServerChecksum,
                    FileName = filesListReturned.FileName,
                    FileSize = filesListReturned.FileSize,
                    CreationTimeStamp = filesListReturned.CreationTimeStamp,
                    OwnerID = filesListReturned.OwnerID,
                    GroupID = filesListReturned.GroupID,
                    OwnerPermissionRWX = filesListReturned.OwnerPermissionRWX,
                    GroupPermissionRWX = filesListReturned.GroupPermissionRWX,
                    GlobalPermissionRWX = filesListReturned.GlobalPermissionRWX,
                    ServerOperationID = filesListReturned.ServerOperationID,
                    CreateDt = filesListReturned.CreateDt,
                    fileAttributesDTO = new FileAttributesDTO()
                    {
                        AppId = AppId,
                        FileID = filesListReturned.FileID,
                        FileName = filesListReturned.FileName,
                        Description = fileAttributes.Description,
                        LastChangedTimeStamp = fileAttributes.LastChangedTimeStamp,
                        LastChangedByUserID = fileAttributes.LastChangedByUserID,
                        NumberAccesses = fileAttributes.NumberAccesses,
                        StreamableFlag = fileAttributes.StreamableFlag,
                        StreamingDataRate = fileAttributes.StreamingDataRate,
                        CreateDt = fileAttributes.CreateDt,
                    },
                    fileMetaDataDTO = new FileMetaDataDTO()
                    {
                        AppId = AppId,
                        FileID = fileMetaData.FileID,
                        FileName = fileMetaData.FileName,
                        Key = fileMetaData.Key,
                        Value = fileMetaData.Value,
                        CreateDt = fileMetaData.CreateDt,
                    }
                };
                filesWithMetaDataReturn.Add(fileToList);
                return filesWithMetaDataReturn;
            }
            else 
            {
                var filesListReturnedByOwnerId = db.Files.Where(File => File.AppId == AppId
                            && File.FileName.StartsWith(FileNameBeginsWith)
                            && File.OwnerID == OwnerByID).FirstOrDefault();

                var fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId
                    && x.FileName.StartsWith(FileNameBeginsWith))
                    .FirstOrDefault();

                var fileMetaData = db.FileMetaDatas.Where(x => x.AppId == AppId
                    && x.FileName.StartsWith(FileNameBeginsWith)
                    && x.Key == metaKey)
                    .FirstOrDefault();

                if (filesListReturnedByOwnerId == null)
                {
                    Console.WriteLine("No Files found to list");
                    return null;
                }
                else if (fileAttributes == null)
                {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                }
                else if (fileMetaData == null)
                {
                    Console.WriteLine($"No File with key {metaKey} found to list");
                    return null;
                }

                FileDTO fileToList = new FileDTO()
                {
                    AppId = AppId,
                    FileID = filesListReturnedByOwnerId.FileID,
                    ServerChecksum = filesListReturnedByOwnerId.ServerChecksum,
                    FileName = filesListReturnedByOwnerId.FileName,
                    FileSize = filesListReturnedByOwnerId.FileSize,
                    CreationTimeStamp = filesListReturnedByOwnerId.CreationTimeStamp,
                    OwnerID = filesListReturnedByOwnerId.OwnerID,
                    GroupID = filesListReturnedByOwnerId.GroupID,
                    OwnerPermissionRWX = filesListReturnedByOwnerId.OwnerPermissionRWX,
                    GroupPermissionRWX = filesListReturnedByOwnerId.GroupPermissionRWX,
                    GlobalPermissionRWX = filesListReturnedByOwnerId.GlobalPermissionRWX,
                    ServerOperationID = filesListReturnedByOwnerId.ServerOperationID,
                    fileAttributesDTO = new FileAttributesDTO()
                    {
                        AppId = AppId,
                        FileID = fileAttributes.FileID,
                        FileName = filesListReturnedByOwnerId.FileName,
                        Description = fileAttributes.Description,
                        LastChangedTimeStamp = fileAttributes.LastChangedTimeStamp,
                        LastChangedByUserID = fileAttributes.LastChangedByUserID,
                        NumberAccesses = fileAttributes.NumberAccesses,
                        StreamableFlag = fileAttributes.StreamableFlag,
                        StreamingDataRate = fileAttributes.StreamingDataRate,
                        CreateDt = fileAttributes.CreateDt,
                    },
                    fileMetaDataDTO = new FileMetaDataDTO()
                    {
                        AppId = AppId,
                        FileID = fileMetaData.FileID,
                        FileName = fileMetaData.FileName,
                        Key = fileMetaData.Key,
                        Value = fileMetaData.Value,
                        CreateDt = fileMetaData.CreateDt,
                    }
                };

                filesWithMetaDataReturn.Add(fileToList);

                return filesWithMetaDataReturn;
            }
        }
        #endregion

        #region getFileList 
        [Authorize("database")]
        [HttpGet, Route("getFileList")]
        public async Task<dynamic> getFileList(int AppId, string FileNameBeginsWith, int OwnerByID)
        {
            List<FileDTO> filesReturn = new List<FileDTO>();

            //Jak X
            if(FileNameBeginsWith.Contains("*"))
            {
                var filesListReturnedByOwnerId = db.Files.Where(File => File.AppId == AppId
                            && File.OwnerID == OwnerByID).FirstOrDefault();

                var fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId)
                    .FirstOrDefault();

                if (filesListReturnedByOwnerId == null)
                {
                    Console.WriteLine("No Files found to list");
                    return null;
                }
                else if (fileAttributes == null)
                {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                }

                FileDTO fileToList = new FileDTO()
                {
                    AppId = AppId,
                    FileID = filesListReturnedByOwnerId.FileID,
                    ServerChecksum = filesListReturnedByOwnerId.ServerChecksum,
                    FileName = filesListReturnedByOwnerId.FileName,
                    FileSize = filesListReturnedByOwnerId.FileSize,
                    CreationTimeStamp = filesListReturnedByOwnerId.CreationTimeStamp,
                    OwnerID = filesListReturnedByOwnerId.OwnerID,
                    GroupID = filesListReturnedByOwnerId.GroupID,
                    OwnerPermissionRWX = filesListReturnedByOwnerId.OwnerPermissionRWX,
                    GroupPermissionRWX = filesListReturnedByOwnerId.GroupPermissionRWX,
                    GlobalPermissionRWX = filesListReturnedByOwnerId.GlobalPermissionRWX,
                    ServerOperationID = filesListReturnedByOwnerId.ServerOperationID,
                    fileAttributesDTO = new FileAttributesDTO()
                    {
                        AppId = AppId,
                        FileID = fileAttributes.FileID,
                        FileName = filesListReturnedByOwnerId.FileName,
                        Description = fileAttributes.Description,
                        LastChangedTimeStamp = fileAttributes.LastChangedTimeStamp,
                        LastChangedByUserID = fileAttributes.LastChangedByUserID,
                        NumberAccesses = fileAttributes.NumberAccesses,
                        StreamableFlag = fileAttributes.StreamableFlag,
                        StreamingDataRate = fileAttributes.StreamingDataRate,
                        CreateDt = fileAttributes.CreateDt,
                    }
                };
                filesReturn.Add(fileToList);

                return filesReturn;
            }
            if (AppId == 10994 || AppId == 11203 || AppId == 11204)
            {

                Console.WriteLine($"JakX Filtering");

                List<Files> filesListReturned = db.Files.Where(File => File.AppId == AppId
                    && File.FileName.StartsWith(FileNameBeginsWith)).ToList();


                List<FileAttributes> fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId
                    && x.FileName.StartsWith(FileNameBeginsWith)).ToList();

                if (filesListReturned == null)
                {
                    Console.WriteLine("No Files found to list");
                    return null;
                }
                else if (fileAttributes == null)
                {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                }


                foreach (var file in filesListReturned)
                {
                    FileAttributesDTO fileAttributesDTO = new FileAttributesDTO();

                    foreach (var fileAttribute in fileAttributes)
                    {

                        fileAttributesDTO = new FileAttributesDTO()
                        {
                            AppId = AppId,
                            FileID = file.FileID,
                            FileName = file.FileName,
                            Description = fileAttribute.Description,
                            LastChangedTimeStamp = fileAttribute.LastChangedTimeStamp,
                            LastChangedByUserID = fileAttribute.LastChangedByUserID,
                            NumberAccesses = fileAttribute.NumberAccesses,
                            StreamableFlag = fileAttribute.StreamableFlag,
                            StreamingDataRate = fileAttribute.StreamingDataRate,
                            CreateDt = fileAttribute.CreateDt,
                        };
                    }


                    filesReturn.Add(new FileDTO()
                    {
                        AppId = AppId,
                        FileID = file.FileID,
                        ServerChecksum = file.ServerChecksum,
                        FileName = file.FileName,
                        FileSize = file.FileSize,
                        CreationTimeStamp = file.CreationTimeStamp,
                        OwnerID = file.OwnerID,
                        GroupID = file.GroupID,
                        OwnerPermissionRWX = file.OwnerPermissionRWX,
                        GroupPermissionRWX = file.GroupPermissionRWX,
                        GlobalPermissionRWX = file.GlobalPermissionRWX,
                        ServerOperationID = file.ServerOperationID,
                        fileAttributesDTO = new FileAttributesDTO() { 
                            AppId = AppId, 
                            FileID = fileAttributesDTO.FileID,
                            FileName = fileAttributesDTO.FileName,
                            Description = fileAttributesDTO.Description,
                            LastChangedTimeStamp = fileAttributesDTO.LastChangedTimeStamp,
                            LastChangedByUserID = fileAttributesDTO.LastChangedByUserID,
                            NumberAccesses = fileAttributesDTO.NumberAccesses,
                            StreamableFlag = fileAttributesDTO.StreamableFlag,
                            StreamingDataRate = fileAttributesDTO.StreamingDataRate,
                            CreateDt = fileAttributesDTO.CreateDt,
                        },
                        CreateDt = file.CreateDt,
                    });
                }

                if (filesReturn == null)
                    return NotFound();

                Console.WriteLine($"filesReturn: {filesReturn.Count()}");

                return filesReturn;
            }

            else if (OwnerByID < 0)
            {
                Console.WriteLine($"OwnerById < 0");
                var filesListReturned = db.Files.Where(File => File.AppId == AppId
                    && File.FileName.StartsWith(FileNameBeginsWith))
                    .FirstOrDefault();

                var fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId
                    && x.FileName.StartsWith(FileNameBeginsWith))
                    .FirstOrDefault();

                if (filesListReturned == null)
                {
                    Console.WriteLine("No Files found to list");
                    return null;
                }
                else if (fileAttributes == null)
                {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                }

                FileDTO fileToList = new FileDTO()
                {
                    AppId = AppId,
                    FileID = filesListReturned.FileID,
                    ServerChecksum = filesListReturned.ServerChecksum,
                    FileName = filesListReturned.FileName,
                    FileSize = filesListReturned.FileSize,
                    CreationTimeStamp = filesListReturned.CreationTimeStamp,
                    OwnerID = filesListReturned.OwnerID,
                    GroupID = filesListReturned.GroupID,
                    OwnerPermissionRWX = filesListReturned.OwnerPermissionRWX,
                    GroupPermissionRWX = filesListReturned.GroupPermissionRWX,
                    GlobalPermissionRWX = filesListReturned.GlobalPermissionRWX,
                    ServerOperationID = filesListReturned.ServerOperationID,
                    CreateDt = filesListReturned.CreateDt,
                    fileAttributesDTO = new FileAttributesDTO()
                    {
                        AppId = AppId,
                        FileID = filesListReturned.FileID,
                        FileName = filesListReturned.FileName,
                        Description = fileAttributes.Description,
                        LastChangedTimeStamp = fileAttributes.LastChangedTimeStamp,
                        LastChangedByUserID = fileAttributes.LastChangedByUserID,
                        NumberAccesses = fileAttributes.NumberAccesses,
                        StreamableFlag = fileAttributes.StreamableFlag,
                        StreamingDataRate = fileAttributes.StreamingDataRate,
                        CreateDt = fileAttributes.CreateDt,
                    }
                };
                filesReturn.Add(fileToList);
                return filesReturn;
            }
            else 
            {
                var filesListReturnedByOwnerId = db.Files.Where(File => File.AppId == AppId
                            && File.FileName.StartsWith(FileNameBeginsWith)
                            && File.OwnerID == OwnerByID).FirstOrDefault();

                var fileAttributes = db.FileAttributes.Where(x => x.AppId == AppId
                    && x.FileName.StartsWith(FileNameBeginsWith))
                    .FirstOrDefault();

                if (filesListReturnedByOwnerId == null)
                {
                    Console.WriteLine("No Files found to list");
                    return null;
                }
                else if (fileAttributes == null)
                {
                    Console.WriteLine("No Files attributes found to list");
                    return null;
                }

                FileDTO fileToList = new FileDTO()
                {
                    AppId = AppId,
                    FileID = filesListReturnedByOwnerId.FileID,
                    ServerChecksum = filesListReturnedByOwnerId.ServerChecksum,
                    FileName = filesListReturnedByOwnerId.FileName,
                    FileSize = filesListReturnedByOwnerId.FileSize,
                    CreationTimeStamp = filesListReturnedByOwnerId.CreationTimeStamp,
                    OwnerID = filesListReturnedByOwnerId.OwnerID,
                    GroupID = filesListReturnedByOwnerId.GroupID,
                    OwnerPermissionRWX = filesListReturnedByOwnerId.OwnerPermissionRWX,
                    GroupPermissionRWX = filesListReturnedByOwnerId.GroupPermissionRWX,
                    GlobalPermissionRWX = filesListReturnedByOwnerId.GlobalPermissionRWX,
                    ServerOperationID = filesListReturnedByOwnerId.ServerOperationID,
                    fileAttributesDTO = new FileAttributesDTO()
                    {
                        AppId = AppId,
                        FileID = fileAttributes.FileID,
                        FileName = filesListReturnedByOwnerId.FileName,
                        Description = fileAttributes.Description,
                        LastChangedTimeStamp = fileAttributes.LastChangedTimeStamp,
                        LastChangedByUserID = fileAttributes.LastChangedByUserID,
                        NumberAccesses = fileAttributes.NumberAccesses,
                        StreamableFlag = fileAttributes.StreamableFlag,
                        StreamingDataRate = fileAttributes.StreamingDataRate,
                        CreateDt = fileAttributes.CreateDt,
                    }
                };
                filesReturn.Add(fileToList);

                return filesReturn;
            }
        }
        #endregion

        #region updateFileAttributes
        [Authorize("database")]
        [HttpPost, Route("updateFileAttributes")]
        public async Task<dynamic> updateFileAttributes([FromBody] FileAttributesDTO request)
        {
            var fileAttributes = db.FileAttributes.FirstOrDefault(x => x.FileID == request.FileID);

            if (fileAttributes == null)
                return StatusCode(404, "No files found with file attributes");

            db.FileAttributes.Attach(fileAttributes);

            FileAttributes newFileAttributes = new FileAttributes()
            {
                Description = request.Description,
                NumberAccesses = request.NumberAccesses,
                LastChangedTimeStamp = request.LastChangedTimeStamp,
                LastChangedByUserID = request.LastChangedByUserID,
                StreamableFlag = request.StreamableFlag,
                StreamingDataRate = request.StreamingDataRate,
                CreateDt = DateTime.UtcNow,
            };
            db.FileAttributes.Add(newFileAttributes);
            db.SaveChanges();

            return Ok("File changed");
        }
        #endregion

        #region getFileAttributes
        [Authorize("database")]
        [HttpGet, Route("getFileAttributes")]
        public async Task<dynamic> getFileAttributes(FileDTO fileAttrReq)
        {
            var results = db.FileAttributes.Where(x => x.FileName == fileAttrReq.FileName);
            return results;
        }
        #endregion

        #region updateFileMetaData
        [Authorize("database")]
        [HttpPost, Route("updateFileMetaData")]
        public async Task<dynamic> updateFileMetaData([FromBody] FileMetaDataDTO request)
        {
            var fileMetaData = db.FileMetaDatas.Where(x => x.Key == request.Key &&
                                                        x.AppId == request.AppId).FirstOrDefault();

            
            if(fileMetaData == null)
            {
                FileMetaData addFileMetaData = new FileMetaData()
                {
                    AppId = request.AppId,
                    FileName = request.FileName,
                    Key = request.Key,
                    Value = request.Value,
                    CreateDt = request.CreateDt,
                };
                db.Entry(addFileMetaData).State = EntityState.Added;
                db.FileMetaDatas.Add(addFileMetaData);

                db.SaveChanges();
                return Ok("File Metadata added");
            } else {
                fileMetaData.AppId = request.AppId;
                fileMetaData.FileName = request.FileName;
                fileMetaData.Key = request.Key;
                fileMetaData.Value = request.Value;
                fileMetaData.ModifiedDt = DateTime.UtcNow;

                db.Entry(fileMetaData).State = EntityState.Modified;
                db.FileMetaDatas.Attach(fileMetaData);

                db.SaveChanges();
                return Ok("File Metadata updated");
            }
        }
        #endregion

        #region getFileMetaData
        [Authorize("database")]
        [HttpPost, Route("getFileMetaData")]
        public async Task<dynamic> getFileMetaData([FromBody] FileDTO fileReq)
        {

            var fileMetaDataResults = db.FileMetaDatas.Where(x => x.AppId == fileReq.AppId
                && x.FileName == fileReq.FileName
                && x.Key == fileReq.fileMetaDataDTO.Key).ToList();

            if (fileMetaDataResults == null)
            {
                Console.WriteLine("No Files found return MetaData for");
                return null;
            }
            else
            {
                Console.WriteLine("FileMetaData Returned! ");

                return fileMetaDataResults;
            }
        }
        #endregion

    }
}
