
(function (window) {
    // 1.将未带校验位的 15（或18）位卡号从右依次编号 1 到 15（18），位于奇数位号上的数字乘以 2。
    // 2.将奇位乘积的个十位全部相加，再加上所有偶数位上的数字。
    // 3.将加法和加上校验位能被 10 整除。
    window.luhmCheck = function (bankno) {
        var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

        var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
        var newArr = new Array();
        for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
            newArr.push(first15Num.substr(i, 1));
        }
        var arrJiShu = new Array();  //奇数位*2的积 <9
        var arrJiShu2 = new Array(); //奇数位*2的积 >9

        var arrOuShu = new Array();  //偶数位数组
        for (var j = 0; j < newArr.length; j++) {
            if ((j + 1) % 2 === 1) {//奇数位
                if (parseInt(newArr[j]) * 2 < 9)
                    arrJiShu.push(parseInt(newArr[j]) * 2);
                else
                    arrJiShu2.push(parseInt(newArr[j]) * 2);
            }
            else //偶数位
                arrOuShu.push(newArr[j]);
        }

        var jishuChild1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
        var jishuChild2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
        for (var h = 0; h < arrJiShu2.length; h++) {
            jishuChild1.push(parseInt(arrJiShu2[h]) % 10);
            jishuChild2.push(parseInt(arrJiShu2[h]) / 10);
        }

        var sumJiShu = 0; //奇数位*2 < 9 的数组之和
        var sumOuShu = 0; //偶数位数组之和
        var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
        var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
        var sumTotal = 0;
        for (var m = 0; m < arrJiShu.length; m++) {
            sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
        }

        for (var n = 0; n < arrOuShu.length; n++) {
            sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
        }

        for (var p = 0; p < jishuChild1.length; p++) {
            sumJiShuChild1 = sumJiShuChild1 + parseInt(jishuChild1[p]);
            sumJiShuChild2 = sumJiShuChild2 + parseInt(jishuChild2[p]);
        }
        //计算总和
        sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

        //计算Luhm值
        var k = parseInt(sumTotal) % 10 === 0 ? 10 : parseInt(sumTotal) % 10;
        var luhm = 10 - k;

        if (lastNum === luhm) {
            //alert("Luhm验证通过");
            return true;
        }
        else {
            //alert("银行卡号必须符合Luhm校验");
            return false;
        }
    };
    //指定路径
    window.setCookie = function (name, value, hour, path) {
        var cookie = name + "=" + encodeURIComponent(value) + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        if (path) {
            cookie += "path=" + path;
        }
        document.cookie = cookie;
    };

    /*当前路径写cookie*/
    window.setCookieCurrentPath = function (name, value, hour) {
        window.setCookie(name, value, hour);
    };
    /*当前路径按组写cookie*/
    window.setCookiesCurrentPath = function (name, value, hour) {
        var cookie = name + "=" + encodeURIComponent(name + "=" + value)
            + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        document.cookie = cookie;
    };
    //读取cookie
    window.getCookie = function (name) {
        var re = new RegExp("(^|;)\\s*(" + name + ")=([^;]*)(;|$)", "i");
        var res = re.exec(document.cookie);
        return res != null ? decodeURIComponent(res[3]) : "";
    };
    /*按组读取cookie*/
    window.getCookies = function (name) {
        var result = {};
        var cookies = getCookie(name);
        var itemsCookies = cookies.split("&");
        for (var item in itemsCookies) {
            if (itemsCookies.hasOwnProperty(item)) {
                var items = itemsCookies[item].split("=");
                if (items.length < 2) continue;
                result[items[0]] = items[1];
            }
        }
        return result;
    };

    window.checkIdcard = function (idcard) {
        var errors = new Array(
        "yes",
        "请检查输入的证件号码是否正确", //"身份证号码位数不对!",
        "请检查输入的证件号码是否正确", //"身份证号码出生日期超出范围或含有非法字符!",
        "请检查输入的证件号码是否正确", //"身份证号码校验错误!",
        "请检查输入的证件号码是否正确" //"身份证地区非法!"
        );

        var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
        var y, jym;
        var s, m;
        var idcardArray = new Array();
        idcard = idcard.replace(/(^\s*)|(\s*$)/g, "");

        idcardArray = idcard.split("");
        //地区检验 
        if (area[parseInt(idcard.substr(0, 2))] == null) return errors[4];
        //身份号码位数及格式检验 
        var ereg;
        switch (idcard.length) {
            case 15:
                if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 === 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
                } else {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
                }
                if (ereg.test(idcard)) return errors[0];
                else return errors[2];
            case 18:
                //18位身份号码检测 
                //出生日期的合法性检查  
                //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
                //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
                if (parseInt(idcard.substr(6, 4)) % 4 === 0 || (parseInt(idcard.substr(6, 4)) % 100 === 0 && parseInt(idcard.substr(6, 4)) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
                } else {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
                }
                if (ereg.test(idcard)) {//测试出生日期的合法性 
                    //计算校验位 
                    s = (parseInt(idcardArray[0]) + parseInt(idcardArray[10])) * 7
                    + (parseInt(idcardArray[1]) + parseInt(idcardArray[11])) * 9
                    + (parseInt(idcardArray[2]) + parseInt(idcardArray[12])) * 10
                    + (parseInt(idcardArray[3]) + parseInt(idcardArray[13])) * 5
                    + (parseInt(idcardArray[4]) + parseInt(idcardArray[14])) * 8
                    + (parseInt(idcardArray[5]) + parseInt(idcardArray[15])) * 4
                    + (parseInt(idcardArray[6]) + parseInt(idcardArray[16])) * 2
                    + parseInt(idcardArray[7]) * 1
                    + parseInt(idcardArray[8]) * 6
                    + parseInt(idcardArray[9]) * 3;
                    y = s % 11;
                    m = "F";
                    jym = "10X98765432";
                    m = jym.substr(y, 1); //判断校验位
                    if (m === idcardArray[17]) return errors[0]; //检测ID的校验位 
                    else return errors[3];
                }
                else return errors[2];
            default:
                return errors[1];
        }
    }
    //获取URL参数
    window.getRequest = function (url) {
        try {
            var theRequest = new Array();
            if (url.indexOf("?") > -1) {
                var pair = url.substr(1).split("&");
                for (var i = 0; i < pair.length; i++) {
                    //var key = pair[i].split("=")[0];
                    var value = decodeURI(pair[i].split("=")[1]);
                    var item = { key: value }
                    theRequest.push(item);
                    //theRequest[pair[i].split("=")[0]] = decodeURI(pair[i].split("=")[1]);
                }
            }
            return theRequest;
        } catch (e) {
            return null;
        }
    };

    // 解决IE7 IE8兼容性问题
    window.parseISO8601 = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
            date = new Date(NaN),
            month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    };

    /**
    * 注册自定义鼠标悬停弹窗事件
    * @param {} className 
    * @returns {} 
    */
    window.myTips = function (className) {
        var tipsNums;
        $("." + className).on("mouseenter", function () {
            var remark = $.trim($(this).text());
            var divName = $(this).prop("id");
            if (remark.length >= 10) {
                tipsNums = layer.tips(remark, "#" + divName, {
                    tips: [1, "#3595CC"],
                    time: 0
                });
            }
        }).on("mouseleave", function () {
            if (tipsNums) {
                layer.close(tipsNums);
            }
        });
    };

    /**
     ** 加法函数，用来得到精确的加法结果
     ** 说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
     ** 调用：accAdd(arg1,arg2)
     ** 返回值：arg1加上arg2的精确结果
     **/
    window.accAdd = function (arg1, arg2) {
        var r1, r2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        var c = Math.abs(r1 - r2);
        var m = Math.pow(10, Math.max(r1, r2));
        if (c > 0) {
            var cm = Math.pow(10, c);
            if (r1 > r2) {
                arg1 = Number(arg1.toString().replace(".", ""));
                arg2 = Number(arg2.toString().replace(".", "")) * cm;
            } else {
                arg1 = Number(arg1.toString().replace(".", "")) * cm;
                arg2 = Number(arg2.toString().replace(".", ""));
            }
        } else {
            arg1 = Number(arg1.toString().replace(".", ""));
            arg2 = Number(arg2.toString().replace(".", ""));
        }
        return (arg1 + arg2) / m;
    }

    /**
     ** 减法函数，用来得到精确的减法结果
     ** 说明：javascript的减法结果会有误差，在两个浮点数相减的时候会比较明显。这个函数返回较为精确的减法结果。
     ** 调用：accSub(arg1,arg2)
     ** 返回值：arg1加上arg2的精确结果
     **/
    window.accSub = function (arg1, arg2) {
        var r1, r2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        var m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
        var n = (r1 >= r2) ? r1 : r2;
        return ((arg1 * m - arg2 * m) / m).toFixed(n);
    }

    /**
    ** 乘法函数，用来得到精确的乘法结果
    ** 说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
    ** 调用：accMul(arg1,arg2)
    ** 返回值：arg1乘以 arg2的精确结果
    **/
    window.accMul = function (arg1, arg2) {
        var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
        try {
            m += s1.split(".")[1].length;
        } catch (e) {
        }
        try {
            m += s2.split(".")[1].length;
        } catch (e) {
        }
        return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    };

    /** 
    ** 除法函数，用来得到精确的除法结果
    ** 说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为精确的除法结果。
    ** 调用：accDiv(arg1,arg2)
    ** 返回值：arg1除以arg2的精确结果
    **/
    window.accDiv = function (arg1, arg2) {
        var t1 = 0, t2 = 0, r1, r2;
        try {
            t1 = arg1.toString().split(".")[1].length;
        } catch (e) {
        }
        try {
            t2 = arg2.toString().split(".")[1].length;
        } catch (e) {
        }
        with (Math) {
            r1 = Number(arg1.toString().replace(".", ""));
            r2 = Number(arg2.toString().replace(".", ""));
            return (r1 / r2) * pow(10, t2 - t1);
        }
    };

    /**
     * 根据航司代码在下拉框中显示对应的航司全名
     * @param {航司代码} code 
     * @param {下拉框ID} selectId 
     * @returns {} 
     */
    window.showCarrierByCode = function (code, selectId) {
        code = $.trim(code.toUpperCase());
        $("#" + selectId).val(code);
    }

    /**
     * 根据城市代码在下拉框中显示对应的城市全名
     * @param {城市代码} code 
     * @param {下拉框ID} selectId 
     * @returns {} 
     */
    window.showCityByCode = function (code, selectId) {
        code = $.trim(code.toUpperCase());
        $("#" + selectId).val(code);
    }

})(window);

(function (window, date) {
    /*
     *参考地址
     *http://www.cnblogs.com/carekee/articles/1678041.html
    */
    //格式化时间
    date.prototype.Format = function (formatStr) {
        var str = formatStr;
        var week = ["日", "一", "二", "三", "四", "五", "六"];

        str = str.replace(/yyyy|YYYY/, this.getFullYear());
        str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : "0" + (this.getYear() % 100));

        var month = this.getMonth() + 1;
        str = str.replace(/MM/, month > 9 ? month.toString() : "0" + month);
        str = str.replace(/M/g, month);

        str = str.replace(/w|W/g, week[this.getDay()]);

        str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : "0" + this.getDate());
        str = str.replace(/d|D/g, this.getDate());

        str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : "0" + this.getHours());
        str = str.replace(/h|H/g, this.getHours());
        str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : "0" + this.getMinutes());
        str = str.replace(/m/g, this.getMinutes());

        str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : "0" + this.getSeconds());
        str = str.replace(/s|S/g, this.getSeconds());

        return str;
    };

    date.prototype.add = function (part, value) {
        value *= 1;
        if (isNaN(value)) {
            value = 0;
        }
        switch (part) {
            case "y":
                this.setFullYear(this.getFullYear() + value);
                break;
            case "m":
                this.setMonth(this.getMonth() + value);
                break;
            case "d":
                this.setDate(this.getDate() + value);
                break;
            case "h":
                this.setHours(this.getHours() + value);
                break;
            case "n":
                this.setMinutes(this.getMinutes() + value);
                break;
            case "s":
                this.setSeconds(this.getSeconds() + value);
                break;
            default:

        }
    }

    //合并对象
    var extend = function (des, src, override) {
        if (src instanceof Array) {
            for (var i = 0, len = src.length; i < len; i++) {
                extend(des, src[i], override);
            }
        }
        for (var item in src) {
            if (src.hasOwnProperty(item)) {
                if (override || !(item in des)) {
                    des[item] = src[item];
                }
            }
        }
        return des;
    };
    /*获取n天的时间区间*/
    window.getRanage = function (o) {
        var obj = {
            count: 0,
            format: "yyyy-MM-dd"
        };
        var current = extend({}, [o, obj]);
        var nowDate = new Date();
        var targetDate = new Date();
        targetDate.setDate(targetDate.getDate() + current.count);
        var one = targetDate.getFullYear() + "-" + (targetDate.getMonth() + 1) + "-" + targetDate.getDate();
        var two = nowDate.getFullYear() + "-" + (nowDate.getMonth() + 1) + "-" + nowDate.getDate();
        var result = {};
        if (current.count < 0) {
            result.lower = one;
            result.upper = two;
        } else {
            result.lower = two;
            result.upper = one;
        }
        return result;
    };
    /*获取本月的开头结尾*/
    function getMonthDays(myMonth) {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        //var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    function formatDate(dateTime) {
        var myyear = dateTime.getFullYear();
        var mymonth = dateTime.getMonth() + 1;
        var myweekday = dateTime.getDate();
        if (mymonth < 10) {
            mymonth = "0" + mymonth;
        }
        if (myweekday < 10) {
            myweekday = "0" + myweekday;
        }
        return (myyear + "-" + mymonth + "-" + myweekday);
    }

    window.getMonthDate = function () {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, nowMonth, 1);
        var monthEndDate = new Date(nowYear, nowMonth, getMonthDays(nowMonth));
        return { lower: formatDate(monthStartDate), upper: formatDate(monthEndDate) };
    };
    //格局化日期：yyyy-MM-dd 
    //获得某月的天数 
    //+---------------------------------------------------  
    //| 求两个时间的天数差 日期格式为 YYYY-MM-dd   
    //+---------------------------------------------------  
    window.daysBetween = function (dateOne, dateTwo) {
        var oneMonth = dateOne.substring(5, dateOne.lastIndexOf("-"));
        var oneDay = dateOne.substring(dateOne.length, dateOne.lastIndexOf("-") + 1);
        var oneYear = dateOne.substring(0, dateOne.indexOf("-"));

        var twoMonth = dateTwo.substring(5, dateTwo.lastIndexOf("-"));
        var twoDay = dateTwo.substring(dateTwo.length, dateTwo.lastIndexOf("-") + 1);
        var twoYear = dateTwo.substring(0, dateTwo.indexOf("-"));

        var cha = ((Date.parse(oneMonth + "/" + oneDay + "/" + oneYear) - Date.parse(twoMonth + "/" + twoDay + "/" + twoYear)) / 86400000);
        return Math.abs(cha);
    };

    // {时间转化:格式yyyy-MM-dd HH:mm:ss 转化精确到秒}
    window.parseSeconds = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s{1}(\d{2}):(\d{2}):(\d{2})\s*$/,
            date = new Date(NaN), month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            date.setHours(parts[4], parts[5], parts[6]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }

    // {时间转化:格式yyyy-MM-dd HH:mm:ss 转化精确到分钟}
    window.parseMinutes = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s{1}(\d{2}):(\d{2}):(\d{2})\s*$/,
            date = new Date(NaN), month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            date.setHours(parts[4], parts[5]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }
})(window, Date);(function (window) {
    // 1.将未带校验位的 15（或18）位卡号从右依次编号 1 到 15（18），位于奇数位号上的数字乘以 2。
    // 2.将奇位乘积的个十位全部相加，再加上所有偶数位上的数字。
    // 3.将加法和加上校验位能被 10 整除。
    window.luhmCheck = function (bankno) {
        var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

        var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
        var newArr = new Array();
        for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
            newArr.push(first15Num.substr(i, 1));
        }
        var arrJiShu = new Array();  //奇数位*2的积 <9
        var arrJiShu2 = new Array(); //奇数位*2的积 >9

        var arrOuShu = new Array();  //偶数位数组
        for (var j = 0; j < newArr.length; j++) {
            if ((j + 1) % 2 === 1) {//奇数位
                if (parseInt(newArr[j]) * 2 < 9)
                    arrJiShu.push(parseInt(newArr[j]) * 2);
                else
                    arrJiShu2.push(parseInt(newArr[j]) * 2);
            }
            else //偶数位
                arrOuShu.push(newArr[j]);
        }

        var jishuChild1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
        var jishuChild2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
        for (var h = 0; h < arrJiShu2.length; h++) {
            jishuChild1.push(parseInt(arrJiShu2[h]) % 10);
            jishuChild2.push(parseInt(arrJiShu2[h]) / 10);
        }

        var sumJiShu = 0; //奇数位*2 < 9 的数组之和
        var sumOuShu = 0; //偶数位数组之和
        var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
        var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
        var sumTotal = 0;
        for (var m = 0; m < arrJiShu.length; m++) {
            sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
        }

        for (var n = 0; n < arrOuShu.length; n++) {
            sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
        }

        for (var p = 0; p < jishuChild1.length; p++) {
            sumJiShuChild1 = sumJiShuChild1 + parseInt(jishuChild1[p]);
            sumJiShuChild2 = sumJiShuChild2 + parseInt(jishuChild2[p]);
        }
        //计算总和
        sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

        //计算Luhm值
        var k = parseInt(sumTotal) % 10 === 0 ? 10 : parseInt(sumTotal) % 10;
        var luhm = 10 - k;

        if (lastNum === luhm) {
            //alert("Luhm验证通过");
            return true;
        }
        else {
            //alert("银行卡号必须符合Luhm校验");
            return false;
        }
    };
    //指定路径
    window.setCookie = function (name, value, hour, path) {
        var cookie = name + "=" + encodeURIComponent(value) + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        if (path) {
            cookie += "path=" + path;
        }
        document.cookie = cookie;
    };

    /*当前路径写cookie*/
    window.setCookieCurrentPath = function (name, value, hour) {
        window.setCookie(name, value, hour);
    };
    /*当前路径按组写cookie*/
    window.setCookiesCurrentPath = function (name, value, hour) {
        var cookie = name + "=" + encodeURIComponent(name + "=" + value)
            + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        document.cookie = cookie;
    };
    //读取cookie
    window.getCookie = function (name) {
        var re = new RegExp("(^|;)\\s*(" + name + ")=([^;]*)(;|$)", "i");
        var res = re.exec(document.cookie);
        return res != null ? decodeURIComponent(res[3]) : "";
    };
    /*按组读取cookie*/
    window.getCookies = function (name) {
        var result = {};
        var cookies = getCookie(name);
        var itemsCookies = cookies.split("&");
        for (var item in itemsCookies) {
            if (itemsCookies.hasOwnProperty(item)) {
                var items = itemsCookies[item].split("=");
                if (items.length < 2) continue;
                result[items[0]] = items[1];
            }
        }
        return result;
    };

    window.checkIdcard = function (idcard) {
        var errors = new Array(
        "yes",
        "请检查输入的证件号码是否正确", //"身份证号码位数不对!",
        "请检查输入的证件号码是否正确", //"身份证号码出生日期超出范围或含有非法字符!",
        "请检查输入的证件号码是否正确", //"身份证号码校验错误!",
        "请检查输入的证件号码是否正确" //"身份证地区非法!"
        );

        var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
        var y, jym;
        var s, m;
        var idcardArray = new Array();
        idcard = idcard.replace(/(^\s*)|(\s*$)/g, "");

        idcardArray = idcard.split("");
        //地区检验 
        if (area[parseInt(idcard.substr(0, 2))] == null) return errors[4];
        //身份号码位数及格式检验 
        var ereg;
        switch (idcard.length) {
            case 15:
                if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 === 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
                } else {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
                }
                if (ereg.test(idcard)) return errors[0];
                else return errors[2];
            case 18:
                //18位身份号码检测 
                //出生日期的合法性检查  
                //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
                //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
                if (parseInt(idcard.substr(6, 4)) % 4 === 0 || (parseInt(idcard.substr(6, 4)) % 100 === 0 && parseInt(idcard.substr(6, 4)) % 4 === 0)) {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
                } else {
                    ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
                }
                if (ereg.test(idcard)) {//测试出生日期的合法性 
                    //计算校验位 
                    s = (parseInt(idcardArray[0]) + parseInt(idcardArray[10])) * 7
                    + (parseInt(idcardArray[1]) + parseInt(idcardArray[11])) * 9
                    + (parseInt(idcardArray[2]) + parseInt(idcardArray[12])) * 10
                    + (parseInt(idcardArray[3]) + parseInt(idcardArray[13])) * 5
                    + (parseInt(idcardArray[4]) + parseInt(idcardArray[14])) * 8
                    + (parseInt(idcardArray[5]) + parseInt(idcardArray[15])) * 4
                    + (parseInt(idcardArray[6]) + parseInt(idcardArray[16])) * 2
                    + parseInt(idcardArray[7]) * 1
                    + parseInt(idcardArray[8]) * 6
                    + parseInt(idcardArray[9]) * 3;
                    y = s % 11;
                    m = "F";
                    jym = "10X98765432";
                    m = jym.substr(y, 1); //判断校验位
                    if (m === idcardArray[17]) return errors[0]; //检测ID的校验位 
                    else return errors[3];
                }
                else return errors[2];
            default:
                return errors[1];
        }
    }
    //获取URL参数
    window.getRequest = function (url) {
        try {
            var theRequest = new Array();
            if (url.indexOf("?") > -1) {
                var pair = url.substr(1).split("&");
                for (var i = 0; i < pair.length; i++) {
                    //var key = pair[i].split("=")[0];
                    var value = decodeURI(pair[i].split("=")[1]);
                    var item = { key: value }
                    theRequest.push(item);
                    //theRequest[pair[i].split("=")[0]] = decodeURI(pair[i].split("=")[1]);
                }
            }
            return theRequest;
        } catch (e) {
            return null;
        }
    };

    // 解决IE7 IE8兼容性问题
    window.parseISO8601 = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
            date = new Date(NaN),
            month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    };

    /**
    * 注册自定义鼠标悬停弹窗事件
    * @param {} className 
    * @returns {} 
    */
    window.myTips = function (className) {
        var tipsNums;
        $("." + className).on("mouseenter", function () {
            var remark = $.trim($(this).text());
            var divName = $(this).prop("id");
            if (remark.length >= 10) {
                tipsNums = layer.tips(remark, "#" + divName, {
                    tips: [1, "#3595CC"],
                    time: 0
                });
            }
        }).on("mouseleave", function () {
            if (tipsNums) {
                layer.close(tipsNums);
            }
        });
    };

    /**
     ** 加法函数，用来得到精确的加法结果
     ** 说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
     ** 调用：accAdd(arg1,arg2)
     ** 返回值：arg1加上arg2的精确结果
     **/
    window.accAdd = function (arg1, arg2) {
        var r1, r2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        var c = Math.abs(r1 - r2);
        var m = Math.pow(10, Math.max(r1, r2));
        if (c > 0) {
            var cm = Math.pow(10, c);
            if (r1 > r2) {
                arg1 = Number(arg1.toString().replace(".", ""));
                arg2 = Number(arg2.toString().replace(".", "")) * cm;
            } else {
                arg1 = Number(arg1.toString().replace(".", "")) * cm;
                arg2 = Number(arg2.toString().replace(".", ""));
            }
        } else {
            arg1 = Number(arg1.toString().replace(".", ""));
            arg2 = Number(arg2.toString().replace(".", ""));
        }
        return (arg1 + arg2) / m;
    }

    /**
     ** 减法函数，用来得到精确的减法结果
     ** 说明：javascript的减法结果会有误差，在两个浮点数相减的时候会比较明显。这个函数返回较为精确的减法结果。
     ** 调用：accSub(arg1,arg2)
     ** 返回值：arg1加上arg2的精确结果
     **/
    window.accSub = function (arg1, arg2) {
        var r1, r2;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        var m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
        var n = (r1 >= r2) ? r1 : r2;
        return ((arg1 * m - arg2 * m) / m).toFixed(n);
    }

    /**
    ** 乘法函数，用来得到精确的乘法结果
    ** 说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
    ** 调用：accMul(arg1,arg2)
    ** 返回值：arg1乘以 arg2的精确结果
    **/
    window.accMul = function (arg1, arg2) {
        var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
        try {
            m += s1.split(".")[1].length;
        } catch (e) {
        }
        try {
            m += s2.split(".")[1].length;
        } catch (e) {
        }
        return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    };

    /** 
    ** 除法函数，用来得到精确的除法结果
    ** 说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为精确的除法结果。
    ** 调用：accDiv(arg1,arg2)
    ** 返回值：arg1除以arg2的精确结果
    **/
    window.accDiv = function (arg1, arg2) {
        var t1 = 0, t2 = 0, r1, r2;
        try {
            t1 = arg1.toString().split(".")[1].length;
        } catch (e) {
        }
        try {
            t2 = arg2.toString().split(".")[1].length;
        } catch (e) {
        }
        with (Math) {
            r1 = Number(arg1.toString().replace(".", ""));
            r2 = Number(arg2.toString().replace(".", ""));
            return (r1 / r2) * pow(10, t2 - t1);
        }
    };

    /**
     * 根据航司代码在下拉框中显示对应的航司全名
     * @param {航司代码} code 
     * @param {下拉框ID} selectId 
     * @returns {} 
     */
    window.showCarrierByCode = function (code, selectId) {
        code = $.trim(code.toUpperCase());
        $("#" + selectId).val(code);
    }

    /**
     * 根据城市代码在下拉框中显示对应的城市全名
     * @param {城市代码} code 
     * @param {下拉框ID} selectId 
     * @returns {} 
     */
    window.showCityByCode = function (code, selectId) {
        code = $.trim(code.toUpperCase());
        $("#" + selectId).val(code);
    }

})(window);

(function (window, date) {
    /*
     *参考地址
     *http://www.cnblogs.com/carekee/articles/1678041.html
    */
    //格式化时间
    date.prototype.Format = function (formatStr) {
        var str = formatStr;
        var week = ["日", "一", "二", "三", "四", "五", "六"];

        str = str.replace(/yyyy|YYYY/, this.getFullYear());
        str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : "0" + (this.getYear() % 100));

        var month = this.getMonth() + 1;
        str = str.replace(/MM/, month > 9 ? month.toString() : "0" + month);
        str = str.replace(/M/g, month);

        str = str.replace(/w|W/g, week[this.getDay()]);

        str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : "0" + this.getDate());
        str = str.replace(/d|D/g, this.getDate());

        str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : "0" + this.getHours());
        str = str.replace(/h|H/g, this.getHours());
        str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : "0" + this.getMinutes());
        str = str.replace(/m/g, this.getMinutes());

        str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : "0" + this.getSeconds());
        str = str.replace(/s|S/g, this.getSeconds());

        return str;
    };

    date.prototype.add = function (part, value) {
        value *= 1;
        if (isNaN(value)) {
            value = 0;
        }
        switch (part) {
            case "y":
                this.setFullYear(this.getFullYear() + value);
                break;
            case "m":
                this.setMonth(this.getMonth() + value);
                break;
            case "d":
                this.setDate(this.getDate() + value);
                break;
            case "h":
                this.setHours(this.getHours() + value);
                break;
            case "n":
                this.setMinutes(this.getMinutes() + value);
                break;
            case "s":
                this.setSeconds(this.getSeconds() + value);
                break;
            default:

        }
    }

    //合并对象
    var extend = function (des, src, override) {
        if (src instanceof Array) {
            for (var i = 0, len = src.length; i < len; i++) {
                extend(des, src[i], override);
            }
        }
        for (var item in src) {
            if (src.hasOwnProperty(item)) {
                if (override || !(item in des)) {
                    des[item] = src[item];
                }
            }
        }
        return des;
    };
    /*获取n天的时间区间*/
    window.getRanage = function (o) {
        var obj = {
            count: 0,
            format: "yyyy-MM-dd"
        };
        var current = extend({}, [o, obj]);
        var nowDate = new Date();
        var targetDate = new Date();
        targetDate.setDate(targetDate.getDate() + current.count);
        var one = targetDate.getFullYear() + "-" + (targetDate.getMonth() + 1) + "-" + targetDate.getDate();
        var two = nowDate.getFullYear() + "-" + (nowDate.getMonth() + 1) + "-" + nowDate.getDate();
        var result = {};
        if (current.count < 0) {
            result.lower = one;
            result.upper = two;
        } else {
            result.lower = two;
            result.upper = one;
        }
        return result;
    };
    /*获取本月的开头结尾*/
    function getMonthDays(myMonth) {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        //var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    function formatDate(dateTime) {
        var myyear = dateTime.getFullYear();
        var mymonth = dateTime.getMonth() + 1;
        var myweekday = dateTime.getDate();
        if (mymonth < 10) {
            mymonth = "0" + mymonth;
        }
        if (myweekday < 10) {
            myweekday = "0" + myweekday;
        }
        return (myyear + "-" + mymonth + "-" + myweekday);
    }

    window.getMonthDate = function () {
        var now = new Date(); //当前日期 
        //var nowDayOfWeek = now.getDay(); //今天本周的第几天 
        //var nowDay = now.getDate(); //当前日 
        var nowMonth = now.getMonth(); //当前月 
        var nowYear = now.getYear(); //当前年 
        nowYear += (nowYear < 2000) ? 1900 : 0;
        var monthStartDate = new Date(nowYear, nowMonth, 1);
        var monthEndDate = new Date(nowYear, nowMonth, getMonthDays(nowMonth));
        return { lower: formatDate(monthStartDate), upper: formatDate(monthEndDate) };
    };
    //格局化日期：yyyy-MM-dd 
    //获得某月的天数 
    //+---------------------------------------------------  
    //| 求两个时间的天数差 日期格式为 YYYY-MM-dd   
    //+---------------------------------------------------  
    window.daysBetween = function (dateOne, dateTwo) {
        var oneMonth = dateOne.substring(5, dateOne.lastIndexOf("-"));
        var oneDay = dateOne.substring(dateOne.length, dateOne.lastIndexOf("-") + 1);
        var oneYear = dateOne.substring(0, dateOne.indexOf("-"));

        var twoMonth = dateTwo.substring(5, dateTwo.lastIndexOf("-"));
        var twoDay = dateTwo.substring(dateTwo.length, dateTwo.lastIndexOf("-") + 1);
        var twoYear = dateTwo.substring(0, dateTwo.indexOf("-"));

        var cha = ((Date.parse(oneMonth + "/" + oneDay + "/" + oneYear) - Date.parse(twoMonth + "/" + twoDay + "/" + twoYear)) / 86400000);
        return Math.abs(cha);
    };

    // {时间转化:格式yyyy-MM-dd HH:mm:ss 转化精确到秒}
    window.parseSeconds = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s{1}(\d{2}):(\d{2}):(\d{2})\s*$/,
            date = new Date(NaN), month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            date.setHours(parts[4], parts[5], parts[6]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }

    // {时间转化:格式yyyy-MM-dd HH:mm:ss 转化精确到分钟}
    window.parseMinutes = function (dateStringInRange) {
        var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s{1}(\d{2}):(\d{2}):(\d{2})\s*$/,
            date = new Date(NaN), month,
            parts = isoExp.exec(dateStringInRange);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            date.setHours(parts[4], parts[5]);
            if (month !== date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }
})(window, Date);

(function ($) {
    $.fn.extend({
        tabControl: function (o) {
            var obj = $.extend(true, {
                className: "curr",       //当前选中class
                isRememberState: false,  //是否记住状态
                isUnload: false,         //页面离开是否删除cookie
                cookieName: "OptionsId", //记住状态Id
                currId: null,            //默认选中Id
                event: "click",
                cookieHour: 1,
                callback: null           //回调函数  
            }, o);
            var $this = $(this);

            function setCurrentClassName(self) {
                $this.each(function () {
                    if ($(this).hasClass(obj.className)) {
                        $(this).removeClass(obj.className);
                    }
                });
                self.addClass(obj.className);
            }

            function setCookie(self) {
                if (obj.isRememberState) {
                    var id = self.attr("id");
                    // cookie写入到当前跟目录 为了确保值一致
                    if (id !== "undefined") {
                        window.setCookie(obj.cookieName, id, obj.cookieHour, "/");
                    }
                }
            }

            function currSelected(currentId) {
                var selectedId = $("#" + currentId + "");
                if (selectedId) {
                    setCurrentClassName(selectedId);
                    setCookie(selectedId);
                }
            }

            if (obj.isRememberState) {
                var cookieId = getCookie(obj.cookieName);
                if (cookieId && cookieId !== "undefined") {
                    currSelected(cookieId);
                }
            }
            if (obj.currId) {
                var cookieId1 = getCookie(obj.cookieName);
                if (cookieId1 && cookieId1 !== "undefined") {
                    currSelected(cookieId1);
                } else {
                    currSelected(obj.currId);
                }

            }
            $this.each(function () {
                $(this).on(obj.event, function () {
                    setCurrentClassName($(this));
                    setCookie($(this));
                });
            });
            if (obj.isUnload) {
                window.onunload = function () {
                    window.setCookieCurrentPath(obj.cookieName, "", obj.cookieHour);
                };
            }
        }
    });

    $.extend({
        ajaxExtend: function (o) {
            var obj = $.extend(true, {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                timeout: 50000,
                error: function (e) {
                    var rel;
                    try {
                        rel = JSON.parse(e.responseText);
                    } catch (ex) {
                        if (e.statusText === "timeout") {
                            //alert("服务器忙");
                        } else if (e) {
                            alert(e.responseText);
                        }
                        return;
                    }
                    if (e.responseText === "") return;
                    if (e.status === 300) {
                        if (rel.Type === "RequireLogon") {
                            if (window.top) {
                                window.top.location.href = rel.Result;
                            } else {
                                window.location.href = rel.Result;
                            }
                            return;
                        } else if (rel.Type === "Unauthorized") {
                            window.location.href = rel.Result;
                            return;
                        } else if (rel.Type === "ExceptionPermission") {
                            if (window.top) {
                                window.top.location.href = "/Unauthorized/ExceptionPermission?message=" + encodeURI(rel.Result);
                            } else {
                                window.location.href = "/Unauthorized/ExceptionPermission?message=" + encodeURI(rel.Result);
                            }
                        }
                    } else if (e.status === 401 && e.statusText === "Unauthorized") {
                        window.location.href = rel.Result;
                        return;
                    }
                }
            }, o);
            $.ajax(obj);
        },
        layerAlert: function (msg, o, ok) {
            //skin样式类名
            var obj = $.extend(true, {
                closeBtn: 1,
                shift: 4,
                shade: [0.1, "#000000"]
            }, o);
            var index = window.layer.alert(msg, obj, function () {
                window.layer.close(index);
                if (ok) {
                    ok();
                }
            });

            //type = type || 1;
            //window.layer.alert({
            //    shade: [0.8, "#000000"],
            //    area: ["auto", "auto"],
            //    dialog: {
            //        msg: msg,
            //        btns: 1,
            //        btn: ["确认"],
            //        type: type
            //    }
            //});
        },
        //msg消息,id吸附层Id 如: #id
        //类型：Number/Array，默认：2
        //tips层的私有参数。支持上右下左四个方向，通过1-4进行方向设定。如tips: 3则表示在元素的下面出现。有时你还可能会定义一些颜色，可以设定tips: [1, '#c00']
        layerTips: function (msg, id, colour, time, tips) {
            time = time || 3000;
            colour = colour || "#C00C00'";
            tips = tips || 2;
            return window.layer.tips(msg, id, {
                tips: [tips, colour],
                time: time
            });
        },
        layerConfirm: function (content, options, yes, no) {
            content = content || "你确定要删除吗";
            options = options || { icon: 3, title: "提示" }
            window.layer.confirm(content, options, function (index) {
                window.layer.close(index);
                if (yes) {
                    yes();
                }
            }, function (index) {
                window.layer.close(index);
                if (no) {
                    no();
                }
            });
        },
        // 弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
        esayuiAlert: function (msgString, title, msgType) {
            title = title || "系统提示";
            msgType = msgType || "info";
            $.messager.alert(title, msgString, msgType);
        },
        showDiv: function (mask, content) {
            document.getElementById(content).style.display = "block";
            document.getElementById(mask).style.display = "block";
            var bgdiv = document.getElementById(mask);
            bgdiv.style.width = document.body.scrollWidth;
            $("#" + mask).height($(document).height());
        },
        closeDiv: function (mask, content) {
            document.getElementById(mask).style.display = "none";
            document.getElementById(content).style.display = "none";
        },
        ajaxLoginExtend: function (e) {
            var rel;
            try {
                rel = JSON.parse(e.responseText);
            } catch (ex) {
                if (e.statusText === "timeout") {
                    alert("服务器忙");
                } else if (e) {
                    alert(e.responseText);
                }
                return;
            }
            if (e.responseText === "") return;
            if (e.status === 300) {
                if (rel.Type === "RequireLogon") {
                    window.top.location.href = rel.Result;
                    return;
                } else if (rel.Type === "Unauthorized") {
                    return;
                }
            } else if (e.status === 401 && e.statusText === "Unauthorized") {
                return;
            }
        },
        // 全选 反选
        allChoose: function (o) {
            var obj = $.extend(true, {
                id: "#id",
                name: "name",
                allSelection: true,
                invertSelection: true
            }, o);
            var $id;
            if (obj.id.substring(1) === "#") {
                $id = obj.id;
            } else {
                $id = "#" + obj.id;
            }
            if (obj.invertSelection) {
                // 全选
                $($id).on("click", function () {
                    var checked = $(this).is(":checked");
                    $("input[name='" + obj.name + "']").prop("checked", checked);
                });
            }
            if (obj.invertSelection) {
                // 反选
                $("input[name='" + obj.name + "']").on("click", function () {
                    var count = $("input[name='" + obj.name + "']").size();
                    var checkedCount = $("input[name='" + obj.name + "']:checked").size();
                    if (count === checkedCount) {
                        $($id).prop("checked", "checked");
                    } else {
                        $($id).removeProp("checked");
                    }
                });
            }
        }
    });

    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
})(jQuery);