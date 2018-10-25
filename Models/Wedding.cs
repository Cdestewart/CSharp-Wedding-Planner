using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeddingPlanning.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }
        [Required(ErrorMessage="Must have a name")]
        public int WedderOneId {get;set;}
        [ForeignKey("WedderOneId")]
        
        public User WedderOne {get;set;}

        [Required(ErrorMessage="Must have a name")]
        public int WedderTwoId {get;set;}
        [ForeignKey("WedderTwoId")]
        
        public User WedderTwo {get;set;}
        

        [Required(ErrorMessage="Must have a Date")]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}
        [Required(ErrorMessage="Must have an address")]

        public string Address {get;set;}

        public List<Attendee> Attendees {get;set;}
        [NotMapped]
        public int AttendeeCount { 
            get
            {
             
                if(Attendees != null)
                {
                    return Attendees.Count();
                }
                else{
                    return 0;
                }
                        
            }
        }
        public Wedding()
        {
            Attendees = new List<Attendee>();
        }
        
    }
}