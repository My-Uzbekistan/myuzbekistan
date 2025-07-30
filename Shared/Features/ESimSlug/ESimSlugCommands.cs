namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record SyncESimSlugCommand() : ICommand<Unit>;