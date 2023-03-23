using SqlSugar;

namespace MyBlog.Utility.ApiResult

{
    public class ApiResultHelp
    {
        public static ApiResult Success(object? Data,string? operation) {
            return new ApiResult
            {
                Code = 200,
                Data = Data,
                Msg = operation+ "操作成功",
                Total = 0
            };
        }

        public static ApiResult Success(object Data,RefAsync<int> Total)
        {
            return new ApiResult
            {
                Code = 200,
                Data = Data,
                Msg = "操作成功",
                Total = Total
            };
        }
        //失败返回的参数
        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Msg = msg,
                Total = 0
            };
        }
    }
}
