namespace EstateMapperLibrary.Models
{
    public enum ResultStatus
    {
        OK = 200,
        CREATED = 201,
        DELETED = 204,
        BADREQUEST = 400,
        UNAUTHORIZED = 401,
        FORBIDDEN = 403,
        NOTFOUND = 404,
        Unprocesable = 422,
        ERROR = 500
    }
}
