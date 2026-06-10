namespace Focus.AI.Application.Exceptions;

public class ProjectAlreadyExistsException : ApplicationException
{
    public ProjectAlreadyExistsException(string localPath) 
        : base($"A project mapping for the path '{localPath}' already exists for this user.")
    {
    }
}
