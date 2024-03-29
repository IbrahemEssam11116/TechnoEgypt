﻿namespace TechnoEgypt.DTOS
{
    public class Certificat
    {
        public int Id { get; set; }
        public string Image_Url { get; set; }
    }
    public class LoginToReturnDto
    {
        public int Id{ get; set; }
        public string school { get; set; }
        public int Group_Id { get; set; }
        public string Group_Name { get; set; }
        public List<Certificat> Certificates { get; set; }
        public List<int> childern { get; set; }
    }
}
