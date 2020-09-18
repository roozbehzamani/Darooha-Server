using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Darooha.Test.IntegrationTests.Providers
{
    public class ModelStateControllerTests : Controller
    {
        public ModelStateControllerTests()
        {
            ControllerContext = (new Mock<ControllerContext>()).Object;
        }
    }
}