﻿using a.spritestudio.attribute;

namespace a.spritestudio.editor.attribute
{
    public class HIDE
        : BasicBooleanAttribute
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="part"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override AttributeBase CreateKeyFrame( SpritePart part, ValueBase value )
        {
            Value v = (Value) value;
            return VisibilityUpdater.Create( !v.on );
        }
    }
}
