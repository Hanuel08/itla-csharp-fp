namespace itla_csharp_fp.Model;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    //public int Age { get; set; }
    
    public string Email { get; set; }
    
    public string Gender { get; set; }
    
    public string Career { get; set; }
    
    public double FinalNote { get; set; }
    
    public string RegistrationNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public string FullName =>
        $"{FirstName} {LastName}";
    
    public int Age { get; set; }

}