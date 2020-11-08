//////////////////////////////////////////////////////////
///
/// Name: SupportData.cs
/// Author: William Kurth
/// Description: 
/// Class:  
/// Base Class: 
/// Notes:
/// 


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RADAR.Models
{

    /// <summary>
    /// Location of report
    /// </summary>
    public class LocationsData
    {
        public string Location { get; set; } 
        public string LocationDescription { get; set; } 
        public int DispOrder { get; set; }
    }

    /// <summary>
    /// Shift incident occurred
    /// </summary>
    public class ShiftData
    {
        public string ShiftName { get; set; }
        public string ShiftDescription { get; set; }
        public int DispOrder { get; set; }
    }

    /// <summary>
    /// Type of safety discrepency found
    /// </summary>
    public class TypeData
    {
        public string ObservationType { get; set; }
        public string ObservationDescription { get; set; }
        public int DispOrder { get; set; }
    }


    /// <summary>
    /// Description of Behavior observed
    /// </summary>
    public class BehaviorData
    {
        public int id { get; set; }
        public string Behavior { get; set; }
        public string BehaviorDescription { get; set; }
        public int DispOrder { get; set; }
    }

    public class ADUserData
    {
        public string email { get; set; }
        public string ename { get; set; }
        public string SAMAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string FullName { get; set; }
    }

    /// <summary>
    /// MVC display widget
    /// </summary>
    public class Widget
    {
        public Widget()
        {
            MentorReview = "";
        }
        private DateTime _createdOn = DateTime.MinValue;
        //public DateTime CreatedOn;
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedOn
        {
            get
            {
                return (_createdOn == DateTime.MinValue) ? DateTime.Now : _createdOn;
            }
            set { _createdOn = value; }
        }
        
        public string Gender { get; set; }

        public string EName { get; set; }

        public string UserName { get; set; }

        public string Shift { get; set; }

        public string Observation { get; set; }

        public string Location { get; set; }

        public string Behavior { get; set;  }

        public string Comments { get; set; }

        public string MentorReview { get; set; }

        public string Safe { get; set; }

        public string intercede { get; set;  }

        public string mentorreviewed { get; set; }
    }


}