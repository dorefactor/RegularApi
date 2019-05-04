using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegularApi.Domain.Model;
using RegularApi.Domain.Services;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class JenkinsDeploymentOrderController : AbstractController
    {
        private readonly ITransformer<DeploymentOrderView, DeploymentOrder> _deploymentOrderTransformer;
        private readonly DeploymentService _deploymentService;

        public JenkinsDeploymentOrderController(ITransformer<DeploymentOrderView, DeploymentOrder> deploymentOrderTransformer,
                                                DeploymentService deploymentService)
        {
            _deploymentOrderTransformer = deploymentOrderTransformer;
            _deploymentService = deploymentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeploymentOrderByRequestIdAsync(string id)
        {
            await _deploymentService.GetByRequestIdAsync(id);

            return Ok();
        }

        private DeploymentRequest BuildDeploymentRequest(string deploymentOrderId)
        {
            return new DeploymentRequest
            {
                RequestId = "DO#" + deploymentOrderId,
                Created = DateTime.UtcNow,
            };
        }
    }
}