using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Services;
using Moq;
using RegularApi.Domain.Model;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Services
{
    public class DeploymentTemplateServiceTest
    {
        private const string TemplateName = "super-template";

        private Mock<IDeploymentTemplateDao> _deploymentTemplateDaoMock;

        private DeploymentTemplateService _service;

        [SetUp]
        public void SetUp()
        {
            _deploymentTemplateDaoMock = new Mock<IDeploymentTemplateDao>();
            _service = new DeploymentTemplateService(new LoggerFactory(), _deploymentTemplateDaoMock.Object);
        }

        [Test]
        public async Task GetDeploymentTemplateByNameTest()
        {
            var template = ModelFactory.BuildDeploymentTemplate(TemplateName);

            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.Some(template));

            var result = await _service.GetDeploymentTemplateByNameAsync(TemplateName);

            Assert.True(result.IsRight);
            var storedTemplate = result.RightAsEnumerable().First();
            
            storedTemplate.Should().BeEquivalentTo(template);
            
            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task GetDeploymentTemplateByNameReturnNoneTest()
        {
            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.None);

            var result = await _service.GetDeploymentTemplateByNameAsync(TemplateName);

            Assert.True(result.IsLeft);
            var expectedError = result.LeftAsEnumerable().First();

            expectedError.Should().Be("Deployment template: " + TemplateName + " not found");

            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task GetDeploymentTemplateThrowsExceptionTest()
        {
            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .Throws<Exception>();

            var result = await _service.GetDeploymentTemplateByNameAsync(TemplateName);

            Assert.True(result.IsLeft);
            var expectedError = result.LeftAsEnumerable().First();

            expectedError.Should().Be("Can't get deployment template: " + TemplateName);

            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();            
        }

        [Test]
        public async Task NewDeploymentTemplateTest()
        {
            var template = ModelFactory.BuildDeploymentTemplate(TemplateName);

            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.None);

            _deploymentTemplateDaoMock.Setup(dao => dao.NewAsync(template))
                .ReturnsAsync(template);
            
            var result = await _service.AddDeploymentTemplateAsync(template);
            
            Assert.True(result.IsRight);
            var expectedTemplate = result.RightAsEnumerable().First();
            
            expectedTemplate.Should().BeEquivalentTo(template);

            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.Verify(dao => dao.NewAsync(template));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();            
        }

        [Test]
        public async Task AddExistingDeploymentTemplateTest()
        {
            var template = ModelFactory.BuildDeploymentTemplate(TemplateName);

            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.Some(template));
            
            var result = await _service.AddDeploymentTemplateAsync(template);
            
            Assert.True(result.IsLeft);
            var expectedError = result.LeftAsEnumerable().First();
            
            expectedError.Should().BeEquivalentTo("Deployment template: " + TemplateName + " already exists");

            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();                        
        }

        [Test]
        public async Task AddDeploymentTemplateThrowsExceptionTest()
        {
            var template = ModelFactory.BuildDeploymentTemplate(TemplateName);

            _deploymentTemplateDaoMock.Setup(dao => dao.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.None);

            _deploymentTemplateDaoMock.Setup(dao => dao.NewAsync(template))
                .Throws<Exception>();
            
            var result = await _service.AddDeploymentTemplateAsync(template);
            
            Assert.True(result.IsLeft);
            var expectedError = result.LeftAsEnumerable().First();
            
            expectedError.Should().BeEquivalentTo("Can't create deployment template: " + TemplateName);

            _deploymentTemplateDaoMock.Verify(dao => dao.GetByNameAsync(TemplateName));
            _deploymentTemplateDaoMock.Verify(dao => dao.NewAsync(template));
            _deploymentTemplateDaoMock.VerifyNoOtherCalls();                        
        }
    }
}