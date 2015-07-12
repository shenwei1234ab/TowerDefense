
function CacheObj()
{

};
var Date=
{
    //行程
    //Trip:new Array()
}

module.exports=CacheObj;





//暂存数据(是否要覆盖)
CacheObj.prototype.saveDate = function saveDate(key,date,ifCover)
{
    if(!Date.hasOwnProperty(key))
    {
        Date[key]=new Array();
    }
    if(ifCover)
    {
        Date[key] = date;
    }
    else
    {
        Date[key].push(date);
    }
}







//清除
CacheObj.prototype.clearDate = function clearDate(key)
{
      Date[key] = [];
}


//
CacheObj.prototype.getDate = function getDate(key)
{
    return  Date[key];
}
