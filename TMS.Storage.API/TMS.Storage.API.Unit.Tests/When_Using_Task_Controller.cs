using API.Controllers;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TMS.Storage.Unit.Tests
{
    public class When_Using_TMS_Task_Controller
    {
        private TmstasksController tmstasksController;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IConfigurationSection> _mockConfSection;

        public When_Using_TMS_Task_Controller()
        {
            _mockConfSection = new Mock<IConfigurationSection>();
            _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "default")]).Returns("mock value");
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(_mockConfSection.Object);
        }

        private void GenerateSubtask(Guid task_id, Guid subtask_id, DbContextOptions<TMSSubTasksContext> subtask_options)
        {
            using (var subtask_context = new TMSSubTasksContext(subtask_options))
            {
                subtask_context.Tmssubtask.Add(
                    new TMSSubtask
                    {
                        Id = subtask_id,
                        Name = "Test task",
                        Description = "doing test",
                        IdTask = task_id
                    });
                subtask_context.SaveChanges();
            }
        }

        private void GenerateTask(Guid task_id, DbContextOptions<TMSTasksContext> task_options)
        {
            using (var task_context = new TMSTasksContext(task_options))
            {
                task_context.Tmstask.Add(
                    new TMStask
                    {
                        Id = task_id,
                        Name = "Test task",
                        Description = "doing test"
                    });
                task_context.SaveChanges();
            }
        }

        [Fact]
        public void Should_Return_200_For_Details_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.Details().Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.IsType<List<TMStask>>(response.Value);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_Details_If_Task_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test1").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test1").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.Details().Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_DetailsTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test2").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test2").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.Details(task_id).Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.IsType<TMStask>(response.Value);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsTask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test3").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test3").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.Details(null).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsTask_If_Task_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test4").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test4").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.Details(task_id).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_DetailsSubTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test5").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test5").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasks(subtask_id).Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.IsType<List<TMSSubtask>>(response.Value);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsSubTask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test6").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test6").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasks(null).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsSubTask_If_Task_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test7").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test7").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasks(subtask_id).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_DetailsSubtasksByTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test8").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test8").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasksByTask(task_id).Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.IsType<List<TMSSubtask>>(response.Value);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsSubtasksByTask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test9").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test9").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasksByTask(null).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DetailsSubtasksByTask_If_Task_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test10").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test10").Options;

            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DetailsSubtasksByTask(task_id).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_CreateTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test11").Options;
            var task_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test11").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test task",
                Description = "doing test"
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateTask(exampleTask).Result as OkResult;
                    Assert.NotNull(response);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_Create_If_Name_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test12").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test12").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = null,
                Description = "doing test"
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateTask(exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_Create_If_Description_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test13").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test13").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = null
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateTask(exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_CreateSubtask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test14").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test14").Options;
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test task",
                Description = "doing test",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateSubtask(task_id, exampleSubtask).Result as OkResult;
                    Assert.NotNull(response);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_CreateSubtask_If_Name_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test15").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test15").Options;
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = null,
                Description = "doing test",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateSubtask(task_id, exampleSubtask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_CreateSubtask_If_Description_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test16").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test16").Options;
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test",
                Description = null,
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateSubtask(task_id, exampleSubtask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_CreateSubtask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test17").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test17").Options;
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test",
                Description = "Doing Test",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.CreateSubtask(null, exampleSubtask).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_EditTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test18").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test18").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = "Doing Test"
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_EditTask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test19").Options;
            var task_id = Guid.NewGuid();
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test19").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = "Doing Test"
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(null, exampleTask).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditTask_If_Name_Not_Found_When_Have_Subtasks()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test20").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test20").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = null,
                Description = "Doing Test"

            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditTask_If_Description_Not_Found_When_Have_Subtasks()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test21").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test21").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = null

            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditTask_If_Name_Not_Found_When_Have_Not_Subtasks()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test22").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test22").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = null,
                Description = "Doing Test",
                State = "inProgress"

            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditTask_If_Description_Not_Found_When_Have_Not_Subtasks()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test23").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test23").Options;
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = null,
                State = "inProgress"

            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditTask_If_Want_Change_Task_With_Subtasks()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test24").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test24").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMStask exampleTask = new TMStask
            {
                Id = task_id,
                Name = "Test",
                Description = "Doing Test",
                State = "Completed"

            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditTask(task_id, exampleTask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_EditSubtask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test25").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test25").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test",
                Description = "Doing Test",
                State = "Completed",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditSubtask(subtask_id, exampleSubtask).Result as OkObjectResult;
                    Assert.NotNull(response);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_EditSubtask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test26").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test26").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test",
                Description = "Doing Test",
                State = "Completed",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditSubtask(null, exampleSubtask).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditSubtask_If_Name_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test27").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test27").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = null,
                Description = "Doing Test",
                State = "Completed",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditSubtask(subtask_id, exampleSubtask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_400_For_EditSubtask_If_Description_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test27").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test27").Options;
            GenerateSubtask(task_id, subtask_id, subtask_options);
            TMSSubtask exampleSubtask = new TMSSubtask
            {
                Id = subtask_id,
                Name = "Test",
                Description = null,
                State = "Completed",
                IdTask = task_id
            };
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.EditSubtask(subtask_id, exampleSubtask).Result as BadRequestObjectResult;
                    Assert.Equal(400, response.StatusCode);
                    Assert.NotNull(response);
                }
            }
        }

        [Fact]
        public void Should_Return_200_For_DeleteTask_If_Task_Is_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test28").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test28").Options;
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DeleteTask(task_id).Result as OkResult;
                    Assert.NotNull(response);
                    Assert.Equal(200, response.StatusCode);
                }
            }
        }

        [Fact]
        public void Should_Return_404_For_DeleteTask_If_Id_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test29").Options;
            var task_id = Guid.NewGuid();
            GenerateTask(task_id, task_options);
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test29").Options;
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DeleteTask(null).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }

        }

        [Fact]
        public void Should_Return_404_For_DeleteTask_If_Task_Not_Found()
        {
            var task_options = new DbContextOptionsBuilder<TMSTasksContext>()
                .UseInMemoryDatabase(databaseName: "Tasks Test29").Options;
            var task_id = Guid.NewGuid();
            var subtask_options = new DbContextOptionsBuilder<TMSSubTasksContext>()
                .UseInMemoryDatabase(databaseName: "Subtasks Test29").Options;
            using (var task_context = new TMSTasksContext(task_options))
            {
                using (var subtask_context = new TMSSubTasksContext(subtask_options))
                {
                    tmstasksController = new TmstasksController(task_context, subtask_context, _mockConfiguration.Object);
                    var response = tmstasksController.DeleteTask(task_id).Result as NotFoundResult;
                    Assert.Equal(404, response.StatusCode);
                    Assert.NotNull(response);
                }
            }

        }
    }
}
