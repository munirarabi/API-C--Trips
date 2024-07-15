
namespace Journey.Api.Filters
{
    internal class ResponseErrorJson
    {
        private IList<string> list;

        public ResponseErrorJson(IList<string> list)
        {
            this.list = list;
        }
    }
}