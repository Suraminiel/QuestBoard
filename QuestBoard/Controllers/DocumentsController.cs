using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Models.Domain;
using QuestBoard.Models.ViewModes;
using QuestBoard.Repositories;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Xml.Linq;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    public class DocumentsController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IDocumentsRepository documentsRepository;

        public DocumentsController(IProjectRepository projectRepository, IDocumentsRepository documentsRepository)
        {
            this.projectRepository = projectRepository;
            this.documentsRepository = documentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid projectID)
        {
            // get all document paths from database
            var projectDocuments = await documentsRepository.GetAllOfThisProject(projectID);
            var currentProject = await projectRepository.GetAsync(projectID);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (projectDocuments != null && currentProject != null)
            {

                DocumentViewModel documents = new DocumentViewModel()
                {
                    ProjectId = projectID,
                    Project = currentProject,
                    docs = new List<SingleDocumentViewModel>(),
                };

                foreach (var document in projectDocuments)
                {
                    documents.docs.Add(new SingleDocumentViewModel
                    {
                        id = document.id,
                        name = document.name,
                        path = document.path,
                        Project = document.Project,
                        ProjectId= document.ProjectId,
                        UserId= document.UserId,

                        // teste ob user der uploader oder projektadmin ist.
                        // Wenn ja dann erlaube User den file zu löschen
                        isAuthorizedToEdit = (currentUserId == document.UserId.ToString() ||                                       // teste ob user der uploader ist
                                              currentProject.AdminUserRights.Contains(Guid.Parse(currentUserId)) ) ? true : false, // teste ob user projektadmin ist
                    });
                }


                // Prüfen, ob der aktuelle User Teil des Projekts ist
                
                if (!currentProject.Users.Any(u => u.Id.ToString() == currentUserId))
                {
                    return Forbid();
                }

                

                return View(documents);
            }
            return BadRequest("loading error");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile document, Guid ProjectId)
        {
            
            //var currentproject;
            if (document != null)
            {
                try
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles\\" + ProjectId.ToString());
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filename = document.FileName;//Path.GetFileName(uploadPath);
                    var filePath = Path.Combine(uploadPath, filename);

                    if (System.IO.File.Exists(filePath))
                    {
                        return BadRequest("Datei existiert bereits");
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }

                    var CurrentUserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    // Safe file Path to database
                    var newdDoc = new Documents
                    {
                        ProjectId = ProjectId,
                        name = filename,
                        path = filePath,
                        UserId= CurrentUserID,
                    };

                    var uploadedDoc = await documentsRepository.AddAsync(newdDoc);

                    if (uploadedDoc != null)
                    {
                        return RedirectToAction("List", new { projectID = ProjectId });
                    }

                    return RedirectToAction("List", new { projectID = ProjectId });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Fehler beim Upload: " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Datei zu groß");
            }
            
           
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid fileId, Guid ProjectId)
        {
            var projectDocuments = await documentsRepository.GetAsync(fileId);
            if (projectDocuments == null)
            {
                return BadRequest("Failed to download file");
            }

            if(!System.IO.File.Exists(projectDocuments.path))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(projectDocuments.path);
            var fileExtension = Path.GetExtension(projectDocuments.path);
            var mimeType = "application/octet-stream";

            if (fileExtension == ".pdf")
            {
                mimeType = "application/pdf";
            }

            if (fileExtension == ".jpg" || fileExtension == ".jpeg")
            {
                mimeType = "image/jpeg";
            }

            if (fileExtension == ".png")
            {
                mimeType = "image/png";
            }

            return File(fileBytes, mimeType, projectDocuments.name);

           // return RedirectToAction("List", new { projectID = ProjectId });
        }


        public async Task<IActionResult> DeleteFile(Guid fileId, Guid ProjectId)
        {

            try
            {
                var document = await documentsRepository.GetAsync(fileId);
                var filepath = document.path;

                // Delete File from server
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    TempData["SuccessMessage"] = "Datei erfolgreich gelöscht";
                }
                else
                {
                    TempData["ErrorMessage"] = "Datei nicht gefunden";
                }

                // Delete Databank Entry
                var deletedDocumentEntry = documentsRepository.DeleteAsync(document.id);

                if (deletedDocumentEntry != null)
                {
                    return RedirectToAction("List", new { projectID = ProjectId });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Fehler beim Löschen: " + ex.Message);
            }


            return RedirectToAction("List", new { projectID = ProjectId });
        }

    }


}
