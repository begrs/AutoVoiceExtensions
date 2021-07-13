using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using Microsoft.Extensions.Logging;

namespace ave_server.Core
{
    public class Voice : IDisposable
    {
        private readonly ILogger<Voice> _logger;
        private const float _minConfidence = 0.80F; // A value between 0 and 1

        private string statusBar = string.Empty;
        private readonly CommandTable _commandTable;
        private SpeechRecognitionEngine _recognitionEngine;
        private bool _isEnabled, _isListening;
        private string? _rejected;
        private bool _longListening;
        private readonly static TimeSpan InitialSilenceDefault = TimeSpan.FromSeconds(5);

        public Voice(ILogger<Voice> logger)
        {
            _logger = logger;
            _commandTable = new CommandTable();
            _recognitionEngine = new SpeechRecognitionEngine();
        }

        public void Initialize()
        {
            InitializeSpeechRecognition();
        }

        private void InitializeSpeechRecognition()
        {
            try
            {
                var c = new Choices(_commandTable.ReadCommands().Select(x => x.HumanReadable).ToArray());
                var gb = new GrammarBuilder(c) { Culture = CultureInfo.InstalledUICulture };
                var g = new Grammar(gb);

                _recognitionEngine.InitialSilenceTimeout = InitialSilenceDefault;
                _recognitionEngine.SpeechHypothesized += OnSpeechHypothesized;
                _recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;
                _recognitionEngine.RecognizeCompleted += OnSpeechRecognized;

                _recognitionEngine.LoadGrammarAsync(g);
                _recognitionEngine.SetInputToDefaultAudioDevice();

                _isEnabled = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during speech initialization");
            }
        }

        public void Run()
        {
            try
            {
                if (!_isEnabled)
                {
                    //TODO: tell use how to set it up
                    //SetupVoiceRecognition();
                    return;
                }
                listen();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during startup.");
            }
        }

        public void Dispose()
        {
            _isEnabled = false;
            _recognitionEngine?.Dispose();
            _commandTable.Dispose();
        }

        private void listen()
        {
            try
            {
                if (!_isListening || _longListening)
                {
                    _isListening = true;
                    _recognitionEngine.RecognizeAsync();
                    statusBar = "I'm listening...";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during RecognizeAsync");
            }
        }

        private void OnSpeechHypothesized(object? sender, SpeechHypothesizedEventArgs e)
        {
            if (string.IsNullOrEmpty(_rejected))
                statusBar = "I'm listening... (" + e.Result.Text + " " + Math.Round(e.Result.Confidence * 100) + "%)";
        }

        private void OnSpeechRecognitionRejected(object? sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (e.Result.Text != "yes" && e.Result.Confidence > 0.5F)
            {
                _rejected = e.Result.Text;
                statusBar = "Did you mean " + e.Result.Text + "? (say yes or no)";
            }
        }

        private void OnSpeechRecognized(object? sender, RecognizeCompletedEventArgs e)
        {
            try
            {
                _recognitionEngine!.RecognizeAsyncStop();
                _isListening = false;

                if (e.Result != null && !string.IsNullOrEmpty(_rejected))
                { // Handle answer to precision question
                    statusBar = string.Empty;

                    if (e.Result.Text == "yes")
                    {
                        //_commands.ExecuteCommand(_rejected);
                    }

                    _rejected = null;
                }
                else if (e.Result != null && e.Result.Text == "what can I say")
                {
                    // Show link to command list
                    //TODO
                    System.Diagnostics.Process.Start("https://github.com/ligershark/VoiceExtension/blob/master/src/Resources/commands.txt");
                    statusBar = string.Empty;
                }
                else if (e.Result != null && e.Result.Confidence > _minConfidence)
                {
                    // Speech matches a command
                    // _commands.ExecuteCommand(e.Result.Text);
                    var props = new Dictionary<string, string> { { "phrase", e.Result.Text } };
                }
                else if (string.IsNullOrEmpty(_rejected))
                { // Speech didn't match a command
                    statusBar = "I didn't quite get that. Please try again.";
                }
                else if (e.Result == null && !string.IsNullOrEmpty(_rejected) && !e.InitialSilenceTimeout)
                { // Keep listening when asked about rejected speech
                    _recognitionEngine.RecognizeAsync();
                }
                else
                { // No match or timeout
                    statusBar = string.Empty;
                }

                if (_longListening)
                {
                    listen();
                }
            }
            catch (Exception ex)
            {
                statusBar = string.Empty;
                _logger.LogError(ex, $"Error during {nameof(OnSpeechRecognized)}");
            }
        }

        public void startQuickMode()
        {
            // start long listening
            _longListening = true;
            _recognitionEngine.InitialSilenceTimeout = TimeSpan.Zero;
            statusBar = "I am listening... (keep)";
        }

        public void stopQuickMode()
        {
            // stop long listening
            _longListening = false;
            _recognitionEngine.InitialSilenceTimeout = InitialSilenceDefault;
            statusBar = string.Empty;
        }
    }
}