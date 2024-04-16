namespace QueueManagementSystem.MVC.Models;

public class ServiceProvider
{
    public int Id {get; set;}

    public string FullNames {get; set;}

    public string Email {get; set;}

    public int ServicePointId {get; set;}

    public ServicePoint ServicePoint {get; set;}

    public string Password {get; set;}

    public bool IsAdmin {get; set;}

}