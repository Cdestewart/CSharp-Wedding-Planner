using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeddingPlanning.Models
{
    public class Attendee
    {

        [Key]
        public int AttendeeId {get;set;}

        public int UserId {get;set;}
        [ForeignKey("UserId")]
        public User User {get;set;}
        public int WeddingId {get;set;}
        [ForeignKey("WeddingId")]
        public Wedding Wedding {get;set;}
        
       

    }
}