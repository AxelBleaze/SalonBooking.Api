namespace SalonBooking.Api.DTOs.Schedule;

public class UpdateDayBlocksRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public List<UpdateTimeBlockDto> Blocks { get; set; } = new();
}