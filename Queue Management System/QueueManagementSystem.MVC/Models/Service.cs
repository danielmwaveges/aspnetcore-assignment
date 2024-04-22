namespace QueueManagementSystem.MVC.Models;
using System.ComponentModel.DataAnnotations;
//details of a service offered at a service point
public class Service
{
    public int Id{get; set;}

    [Required]
    public string Name{get; set;}

    [Required]
    public string Description{get; set;}

}

