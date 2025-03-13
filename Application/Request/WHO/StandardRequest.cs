using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.WHO
{
    public class StandardRequest
    {
        public int PregnancyWeek { get; set; }
        public double? HeadCircumference { get; set; }
        //chu vi vong dau 
        public double? HeadCircumferenceMin { get; set; }
        public double? HeadCircumferenceMax { get; set; }
        //can nang
        public double? WeightMin { get; set; }
        public double? WeightMax { get; set; }
        //chieu dai tu dau den chan
        public double? LenghtMin { get; set; }
        public double? LenghtMax { get; set; }
        //Duong kinh luong dinh
        public double? BPDMin { get; set; }
        public double? BPDMax { get; set; }
        //Chu vi vong bung 
        public double? ACMin { get; set; }
        public double? ACMax { get; set; }
        //chieu dai xuong dui
        public double? FLMin { get; set; }
        public double? FLMax { get; set; }
        //nhip tim
        public double? HearRateMin { get; set; }

        public double? HearRateMax { get; set; }

    }
}
