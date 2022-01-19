using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Entities;

public abstract class Entity
{
    [Key] public int Id { get; set; }
}