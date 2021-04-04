export class ArrayHelper
{
    public static removeItem(array, value) {
        var index = array.indexOf(value);
        if (index > -1) {
          array.splice(index, 1);
        }
        return array;
      }

}