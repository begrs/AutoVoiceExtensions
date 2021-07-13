using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Speech;
using ave_server.Core;

namespace ave_server.Controllers
{
    [ApiController]
    [Route("ave-server")]
    public class ListeningController : ControllerBase
    {
        private readonly ILogger<ListeningController> _logger;
        private Lazy<Voice> _voice;

        public ListeningController(ILogger<ListeningController> logger)
        {
            _logger = logger;
            _voice = NewLazyVoiceInstance();
        }

        [HttpGet]
        [Route("listening/start")]
        public IActionResult Start()
        {
            if (!_voice.IsValueCreated)
            {
                return BadRequest("Activate has to be called first.");
            }
            _voice.Value.startQuickMode();
            return Accepted();
        }

        [HttpGet]
        [Route("listening/stop")]
        public void Stop()
        {
            // no need to stop if it was never started
            if (_voice.IsValueCreated)
                _voice.Value.stopQuickMode();
        }

        [HttpGet]
        [Route("activate")]
        public void Activate()
        {
            _voice.Value.Initialize();
            _voice.Value.Run();
        }

        [HttpGet]
        [Route("deactivate")]
        public void Deactivate()
        {
            // no need to dispose if it was never created
            if (_voice.IsValueCreated)
            {
                _voice.Value.Dispose();
                _voice = NewLazyVoiceInstance();
            }
        }

        private Lazy<Voice> NewLazyVoiceInstance()
        {
            var services = this.HttpContext.RequestServices;
            var voice = (Lazy<Voice>?)services?.GetService(typeof(Lazy<Voice>));
            if (voice is null) throw new InvalidOperationException(nameof(ListeningController));
            return voice;
        }

        [HttpGet]
        [Route("reconfigure")]
        public void Reconfigure()
        {

        }
    }
}
