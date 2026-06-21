using Behaviour.Domain.Enums;

namespace Behaviour.Contracts;

public record CreateBehaviourTemplateRequest(string Name, List<CreateBehaviourItem> Items);

public record CreateBehaviourItem(string Name, int DisplayOrder, BehaviourInputType InputType);

public record SubmitBehaviourSheetRequest(
    Guid StudentId,
    int Month,
    int Year,
    List<BehaviourEntryInput> Entries);

public record BehaviourEntryInput(int DayNo, Guid BehaviourItemId, string? Value);

public record ReviewBehaviourSheetRequest(Guid SheetId, string? Remarks);
