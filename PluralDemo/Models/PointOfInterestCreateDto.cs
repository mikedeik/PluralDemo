using System;
using System.ComponentModel.DataAnnotations;

namespace PluralDemo.Models
{
	public class PointOfInterestCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

