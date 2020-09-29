using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using TMS.Report.API.Controllers;
using TMS.Report.API.Models;
using Xunit;

namespace TMS.Report.Api.Unit.Test
{
    public class When_Using_CSV_Controller
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IConfigurationSection> _mockConfSection;
        private CSVController csvController;
        public When_Using_CSV_Controller()
        {
            _mockConfSection = new Mock<IConfigurationSection>();
            _mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "default")]).Returns("mock value");
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(_mockConfSection.Object);
        }

        [Fact]
        public void Should_Return_200_For_Download_If_Task_Is_Found()
        {
            List<CSVModel> exampleListCSV = new List<CSVModel>
            {
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 1",
                    Description = "Doing task 1",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "Completed"
                },
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 2",
                    Description = "Doing task 2",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "Planned"
                },
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 3",
                    Description = "Doing task 3",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "inProgress"
                }
            };
            DateTime exampleDate = DateTime.Now;
            csvController = new CSVController(_mockConfiguration.Object, exampleListCSV);
            var response = csvController.Download(exampleDate).Result as PhysicalFileResult;
            Assert.NotNull(response);
            Assert.IsType<PhysicalFileResult>(response);
        }

        [Fact]
        public void Should_Return_404_For_Download_If_DateTime_Is_Not_Found()
        {
            List<CSVModel> exampleListCSV = new List<CSVModel>
            {
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 1",
                    Description = "Doing task 1",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "Completed"
                },
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 2",
                    Description = "Doing task 2",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "Planned"
                },
                new CSVModel
                {
                    Id = new Guid(),
                    Name = "Task 3",
                    Description = "Doing task 3",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    State = "inProgress"
                }
            };
            DateTime? exampleDate = null;
            csvController = new CSVController(_mockConfiguration.Object, exampleListCSV);
            var response = csvController.Download(exampleDate).Result as NotFoundResult;
            Assert.Equal(404, response.StatusCode);
            Assert.NotNull(response);
        }
    }
}
