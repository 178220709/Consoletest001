using System;
using System.Collections.Generic;

namespace MyProject.MyHtmlAgility.SpiderCommon
{
    /// <summary>
    /// 同步远程的数据 使用的实体
    /// </summary>
    public  class SpiderRestDto
    {
        public int code { get; set; }
        public int count { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

        public string msg { get; set; }

        public IList<SpiderRestEntity> rows { get; set; }
    }

    public class SpiderRestEntity
    {
        public string url { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public string flag { get; set; }
        public float weight { get; set; }
        public DateTime addedTime { get; set; }
    }
}


/*ex: 
 * {
    "code": 0,
    "count": 220,
    "msg": "this is sp_youmin list ",
    "rows": [{
        "_id": "552fae5e12623f649f2dbec4",
        "url": "http://www.gamersky.com/ent/201501/508132.shtml",
        "title": "顽皮狗画师绘制《旺达与巨像》 动态图观看全过程",
        "content": "<p>hah</p>"
    }],
    "pageSize": "1",
    "pageIndex": "1"
}
 * 
 */