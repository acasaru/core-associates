namespace Interview.Application.Interfaces
{
    public interface IApplicationSerializationService
    {
        string Serialize<TObject>(TObject objectToSerialize);
        TObject Deserialize<TObject>(string jsonRepresentation);
    }
}
