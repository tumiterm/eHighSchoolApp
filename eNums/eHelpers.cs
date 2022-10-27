using System.ComponentModel.DataAnnotations;

namespace SchoolApp.eNums
{
    public enum eGrade
    {
        [Display(Name = "Grade 9")]
        Grade9,
        [Display(Name = "Grade 10")]
        Grade10,
        [Display(Name = "Grade 11")]
        Grade11,
        [Display(Name = "Grade 12")]
        Grade12
    }

    public enum eResponseType
    {
        General,
        TrueFalse,
        MultipleChoice
    }

    public enum eAvailability
    {
        [Display(Name="Not Available")]
        Not,
        Available,
        Unsure
    }

    public enum eMessageType
    {
        [Display(Name = "General Message")]
        General,
        Enquiry,
        Comment,
        Complaint,
        [Display(Name = "Sick Note Submission")]
        Sicknote
    }

    public enum eParentAttachment
    {
        None,
        Sicknote,
        Reciept,
        [Display(Name = "Birth Certificate")]
        Birthcertificate
    }

    public enum eUrgency
    {
        [Display(Name = "Extremely Urgent")]
        Extremely,
        [Display(Name = "Mildly Urgent")]
        Mildly,
        [Display(Name = "Not Urgent")]
        Not
    }

    public enum eRegisterAs
    {
        Parent,Teacher,Learner
    }

    public enum eClass
    {
        [Display(Name = "Classroom A")]
        ClassroomA,
        [Display(Name = "Classroom B")]
        ClassroomB,
        [Display(Name = "Classroom C")]
        ClassroomC,
        [Display(Name = "Classroom D")]
        ClassroomD,
        [Display(Name = "Classroom E")]
        ClassroomE,
        [Display(Name = "Classroom F")]
        ClassroomF,
        [Display(Name = "Classroom G")]
        ClassroomG,
        [Display(Name = "Classroom H")]
        ClassroomH,
        [Display(Name = "Examination Hall")]
        ExamHall,

    }

    public enum eAttandanceCycle
    {
        Weekly,Monthly,
        BiWeekly
    }
    public enum eRole
    {
        None,
        Learner,
        Parent,
        Teacher,
        Admin
    }

    public enum eStatus
    {
        Active,
        Rescheduled,
        Cancelled,
        Completed
    }
    public enum eEmploymentType
    {
        Contract,
        Permanent,
        Internship,
        InService,
        Applicant

    }
    public enum eAssesType
    {
        Assignment,
        [Display(Name = "Class Test")]
        ClassTest,
        Test,
        [Display(Name = "Re-Assessment")]
        ReAssessment,
        Exam
    }

    public enum eTitle
    {
        Mr,Miss,Ms,Dr,Prof,Bishop,Pastor,Cllr,Lady,Lord,
        General,Captain,Earl,Sir,Mx
    }

    public enum eSubject
    {
        [Display(Name = "Afrikaans Home language")]
        AfrikaansHomeLanguage,
        [Display(Name = "Afrikaans First Additional Language")]
        AfrikaansFirstAdditionalLanguage,
        [Display(Name = "English Home Language")]
        EnglishHomeLanguage,
        [Display(Name = "English First Additional Language")]
        EnglishFirstAdditionalLanguage,
        [Display(Name = "IsiNdebele Home Language")]
        IsiNdebeleHomeLanguage,
        [Display(Name = "IsiNdebele First Additional Language")]
        IsiNdebeleFirstAdditionalLanguage,
        [Display(Name = "IsiXhosaHomeLanguage")]
        IsiXhosaHomeLanguage,
        [Display(Name = "IsiXhosa First Additional Language")]
        IsiXhosaFirstAdditionalLanguage,
        [Display(Name = "IsiZulu Home Language")]
        IsiZuluHomeLanguage,
        [Display(Name = "IsiZulu First Additional Language")]
        IsiZuluFirstAdditionalLanguage,
        [Display(Name = "Sepedi Home Language")]
        SepediHomeLanguage,
        [Display(Name = "Sepedi First Additional Language")]
        SepediFirstAdditionalLanguage,
        [Display(Name = "Sesotho Home Language")]
        SesothoHomeLanguage,
        [Display(Name = "Sesotho First Additional Language")]
        SesothoFirstAdditionalLanguage,
        [Display(Name = "Setswana Home Language")]
        SetswanaHomeLanguage,
        [Display(Name = "Setswana First Additional Language")]
        SetswanaFAL,
        [Display(Name = "SiSwati Home Language")]
        SiSwatiHomeLanguage,
        [Display(Name = "SiSwati First Additional Language")]
        SiSwatiFirstAdditionalLanguage,
        [Display(Name = "Tshivenda Home Language")]
        TshivendaHomeLanguage,
        [Display(Name = "Tshivenda First Additional Language")]
        TshivendaFirstAdditionalLanguage,
        [Display(Name = "Xitsonga Home Language")]
        XitsongaHomeLanguage,
        [Display(Name = "Xitsonga First Additional Language")]
        XitsongaFirstAdditionalLanguage,
        [Display(Name = "Agricultural Sciences")]
        AgriculturalSciences,
        Accounting,
        [Display(Name = "Business Studies")]
        BusinessStudies,
        Economics,
        Geography,
        History,
        Tourism,
        Mechanical,
        Physics,
        Electrical,
        [Display(Name = "Engineering Graphics & Design")]
        EGD,
        [Display(Name = "Religion Studies")]
        ReligionStudies,
        [Display(Name = "Computer Applications Technology")]
        ComputerApplicationsTechnology,
        [Display(Name = "Information Technology")]
        InformationTechnology,
        [Display(Name = "Life Sciences")]
        LifeSciences,
        [Display(Name = "Mathematical Literacy")]
        MathematicalLiteracy,
        Mathematics,
        [Display(Name = "Physical Sciences")]
        PhysicalSciences,
        [Display(Name = "Life Orientation")]
        LO



    }

    public enum eRace
    {
        African, White, Asian, Colored, Indian, Other
    }

    public enum eQualificationStatus
    {
        Completed,
        InProgress,
        Incomplete,
    }

    public enum eLanguage
    {
        Setswana, Afrikaans, Sesotho, Sepedi, IsiXhosa, IsiZulu,
        Siswati, XiTsonga, XiVenda, IsiNdebele, English
    }

    public enum eIntake
    {
        [Display(Name = "First Intake")]
        FirstIntake,
        [Display(Name = "Second Intake")]
        SecondIntake,
        [Display(Name = "Third Intake")]
        ThirdIntake
    }
    public enum eGender
    {
        Male,
        Female
    }
    public enum eNationality
    {
        [Display(Name = "South Africa")]
        SouthAfrica,
        Namibia,
        Lesotho,Swaziland, Angola, Botswana, Ghana, 
        Nigeria, Zimbabwe,Mozambique
    }
    public enum eDisability
    {
        [Display(Name = "Communication (Talk/Listen)")]
        Communication,
        [Display(Name = "Emotional (Behaviour/Psychological)")]
        Emotional,
        [Display(Name = "Hearing (Even with Hearing Aid)")]
        Hearing,
        [Display(Name = "Intellectual (Learning etc)")]
        Intellectual,
        [Display(Name = "Multiple Disabled")]
        Multiple,
        Physical,
        Sight,
        None
    }
    public enum eAddressType
    {
        Postal,
        Physical
    }

    public enum eType
    {
        [Display(Name = "Private School")]
        Private,
        [Display(Name = "Public School")]
        Public,
        [Display(Name = "Board School")]
        Boarding,
        [Display(Name = "Charter School")]
        Charter,
        [Display(Name = "Alternative Learning School")]
        AlternativeLearninSchools,
        [Display(Name = "Special Needs School")]
        SpecialNeeds,
        [Display(Name = "International School")]
        InternationalSchools,
        [Display(Name = "Home Schooling")]
        HomeSchooling,
        [Display(Name = "Virtual School")]
        Virtual
    }
    public enum eCitizen
    {
        Yes,No
    }

    public enum eDocumentType
    {
        [Display(Name = "Academic Report")]
        AcademicReport,

        [Display(Name = "Transfer Card")]
        TransferCard,

        [Display(Name = "Birth Certificate/ID/Passport")]
        BirthCertificate
    }

    public enum eSelection
    {
        Yes, No
    }

    public enum eAttachment
    {
        [Display(Name = "Contract of Employment")]
        Contract,
        Qualification,
        [Display(Name = "Proof of Banking")]
        BankingDetails,
        ERP5


    }

    public enum eEmployeeStatus
    {
        [Display(Name = "Is Active")]
        IsActive,
        Suspended,
        Retired
    }

    public enum eEmployeeType
    {
        Educator,
        Principal,
        [Display(Name = "Student Teacher")]
        StudentTeacher,
        Cleaner,
        Security,
        [Display(Name = "General Worker")]
        GeneralWorker
    }

    public enum eQualificationType
    {
        Certificate,
        Diploma,
        Bachelors,
        Honours,
        Masters,
        PhD

    }
    public enum eProvince
    {
        Mpumalanga,
        [Display(Name = "KwaZulu-Natal")]
        KZN,

        [Display(Name = "Eastern Cape")]
        EasternCape,

        [Display(Name = "Western Cape")]
        WesternCape,

        [Display(Name = "North West")]
        NorthWest,

        [Display(Name = "Northern Cape")]
        NorthernCape,

        [Display(Name = "Free State")]
        FreeState,

        Gauteng
    }

    public enum eRelationship
    {
        Mother, Father, Sister, Brother, Aunt, Uncle,
        Grandmother,Grandfather, Niece, Guardian, Other

    }
}
