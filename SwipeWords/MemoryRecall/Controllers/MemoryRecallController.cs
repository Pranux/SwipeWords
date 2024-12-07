using Microsoft.AspNetCore.Mvc;
using SwipeWords.MemoryRecall.Services;

namespace SwipeWords.MemoryRecall.Controllers;

[ApiController]
[Route("api/memory-recall")]
public class MemoryRecallController : ControllerBase
{
    private readonly MemoryRecallService _memoryRecallService;

    public MemoryRecallController(MemoryRecallService memoryRecallService)
    {
        _memoryRecallService = memoryRecallService;
    }

    [HttpPost("fetch-and-process")]
    public async Task<IActionResult> FetchAndProcess([FromQuery] int wordCount = 200, [FromQuery] double placeholderPercentage = 25.0)
    {
        if (wordCount <= 0 || placeholderPercentage < 0 || placeholderPercentage > 100)
        {
            return BadRequest("Invalid parameters. Ensure wordCount > 0 and 0 <= placeholderPercentage <= 100.");
        }

        var (textId, originalText) = await _memoryRecallService.FetchAndSaveTextAsync(wordCount, placeholderPercentage / 100);

        return Ok(new
        {
            textId,
            originalText
        });
    }

    [HttpGet("get-placeholder-text/{id}")]
    public IActionResult GetPlaceholderText(Guid id)
    {
        try
        {
            var placeholderText = _memoryRecallService.GetTextWithPlaceholders(id);
            return Ok(new { placeholderText });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("submit-recall")]
    public IActionResult SubmitRecall(Guid recallId, [FromBody] List<string> userGuesses)
    {
        try
        {
            var (score, correctWords) = _memoryRecallService.CompareUserGuesses(recallId, userGuesses);

            return Ok(new
            {
                score,
                correctWords
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}