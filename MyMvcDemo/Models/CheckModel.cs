namespace MyMvcDemo.Models
{
    public class CheckModel
    {
        public string signature { get; set; } //微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。
        public string timestamp { get; set; } //时间戳
        public string nonce { get; set; } //随机数
        public string echostr { get; set; } //随机字符串
    }
}