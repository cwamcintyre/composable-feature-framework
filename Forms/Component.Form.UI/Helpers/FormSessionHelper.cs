using System;

namespace Component.Form.UI.Helpers;

public static class FormSessionHelper
{
    public const string ApplicantIdKey = "applicantId";

    public static string GetApplicantId(ISession session) 
    {
        var applicantId = session.GetString(ApplicantIdKey);
        if (String.IsNullOrEmpty(applicantId)) 
        {
            throw new ApplicationException("No application session created for user");
        }

        return applicantId;
    }

    public static void SetApplicantId(ISession session, string applicantId) 
    {
        session.SetString(ApplicantIdKey, applicantId);
    }

    public static void ClearApplicantId(ISession session) 
    {
        session.Remove(ApplicantIdKey);
    }

    public static string GenerateApplicantId()
    {
        return Guid.NewGuid().ToString();
    }
}
