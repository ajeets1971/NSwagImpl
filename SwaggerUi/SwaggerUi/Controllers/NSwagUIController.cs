using NSwag;
using NSwag.CodeGeneration.CodeGenerators.CSharp;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SwaggerUi.Controllers
{
    /// <summary>
    /// NSwagUI Controller
    /// </summary>      
    public class NSwagUIController : Controller
    {
        // GET: NSwagUI
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// generateWebApiToSwaggerGenerator
        /// </summary>
        /// <returns></returns>
        public ActionResult generateWebApiToSwaggerGenerator()
        {
            try
            {
                //Controller name for which want to generate swagger specification
                var controllers = new[] { typeof(HomeController), typeof(CompanyController) };
                var settings = new WebApiToSwaggerGeneratorSettings
                {
                    DefaultUrlTemplate = "api/{controller}/{action}/{id}"
                };

                var generator = new WebApiToSwaggerGenerator(settings);
                var service = generator.GenerateForControllers(controllers, excludedMethodName: "Swagger");
                
                TempData["SwaggerJson"] = service.ToJson();
                return (RedirectToAction("Index"));
            }
            catch (Exception ex)
            {
                TempData["SwaggerJson"] = ex.Message;
                return (RedirectToAction("Index"));
            }
        }

        /// <summary>
        /// generateWebApiAssemblyToSwaggerGenerator
        /// </summary>
        /// <returns></returns>
        public ActionResult generateWebApiAssemblyToSwaggerGenerator()
        {
            try
            {
                string assemblyFile = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                string[] AssemblyPaths = { assemblyFile };
                var settings = new WebApiAssemblyToSwaggerGeneratorSettings
                {
                    AssemblyPaths = AssemblyPaths,
                    DefaultUrlTemplate = "api/{controller}/{action}/{id}"
                };

                var generator = new WebApiAssemblyToSwaggerGenerator(settings);
                var service = generator.GenerateForControllers(generator.GetControllerClasses());
                
                TempData["SwaggerJson"] = service.ToJson();
                return (RedirectToAction("Index"));
            }
            catch (Exception ex)
            {
                TempData["SwaggerJson"] = ex.Message;
                return (RedirectToAction("Index"));
            }
        }

        /// <summary>
        /// generateSwaggerToCSharpClientGenerator
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult generateSwaggerToCSharpClientGenerator(FormCollection collection)
        {
            try
            {
                string JsonUrl = collection["txtUrl"];
                var service = SwaggerService.FromUrl(JsonUrl);

                var settings = new SwaggerToCSharpClientGeneratorSettings
                {
                    ClassName = "MyClass"
                };

                var generator = new SwaggerToCSharpClientGenerator(service, settings);
                var code = generator.GenerateFile();
                
                TempData["CSharpCode"] = code;
                return (RedirectToAction("Index"));
            }
            catch (Exception ex)
            {
                TempData["CSharpCode"] = ex.Message;
                return (RedirectToAction("Index"));
            }
        }

        /// <summary>
        /// generateSwaggerToCSharpWebApiControllerGenerator
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult generateSwaggerToCSharpWebApiControllerGenerator(FormCollection collection)
        {
            try
            {
                string JsonUrl = collection["txtUrlWebApiController"];
                var service = SwaggerService.FromUrl(JsonUrl);

                var settings = new SwaggerToCSharpWebApiControllerGeneratorSettings
                {
                    ClassName = "MyClass"
                };
                
                var generator = new SwaggerToCSharpWebApiControllerGenerator(service, settings);
                var code = generator.GenerateFile();

                TempData["CSharpCode"] = code;
                return (RedirectToAction("Index"));
            }
            catch (Exception ex)
            {
                TempData["CSharpCode"] = ex.Message;
                return (RedirectToAction("Index"));
            }
        }
    }
}