namespace my.mcp.server.Document;

public interface IHrmDocumentService
{
    Task<IEnumerable<HrmDocumentInfo>> GetBenefitPlanDocumentsAsync(CancellationToken cancellationToken);
}

public class HrmDocumentInfo
{
    public string DocumentId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}

// Mock implementation (no Azure needed for now)
public class MockHrmDocumentService : IHrmDocumentService
{
    public Task<IEnumerable<HrmDocumentInfo>> GetBenefitPlanDocumentsAsync(CancellationToken cancellationToken)
    {
        var docs = new List<HrmDocumentInfo>
        {
            new() { DocumentId = "medical-plan", Title = "Medical Plan", Description = "Health insurance coverage details", FileName = "medical-plan.pdf" },
            new() { DocumentId = "pto-policy",   Title = "PTO Policy",   Description = "Paid time off rules and accrual",   FileName = "pto-policy.pdf"   },
            new() { DocumentId = "benefits",     Title = "Benefits Guide", Description = "Full employee benefits overview", FileName = "benefits.pdf"     }
        };
        return Task.FromResult<IEnumerable<HrmDocumentInfo>>(docs);
    }
}