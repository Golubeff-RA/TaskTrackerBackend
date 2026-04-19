namespace YourApp.DTOs.Tasks
{
    public class BlockTaskDto
    {
        /// <summary>
        /// Unix timestamp (ms) до которого задача заблокирована. Null = без срока.
        /// </summary>
        public long? BlockedUntilMs { get; set; }
    }
}