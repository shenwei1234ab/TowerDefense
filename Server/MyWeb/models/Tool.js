


function ToolObj()
{

};


module.exports = ToolObj;


ToolObj.prototype.ConvertToArray = function ConvertToArray(value,desArray)
{
    //检查
    var type = typeof(value);
    if(type == 'undefined')
    {
        return desArray;
    }
    if(type == 'string')
    {
        desArray.push(value);
    }
    else
    {
        desArray = value.concat;
    }
}