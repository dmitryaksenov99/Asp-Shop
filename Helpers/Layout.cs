using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Helpers
{
    public static class Layout
    {
        public static Viewport GetViewport()
        {
            return new Viewport
            {
                Layout = LayoutType.Fit.ToString(),
                Items =
                {
                    new Panel
                    {
                        BodyCls = "panelContainer",
                        Scrollable = ScrollableOption.Vertical,
                        Items =
                        {
                            new Panel
                            {

                            },
                            new Panel
                            {
                                Layout = LayoutType.Column.ToString(),
                                BodyCls = "bodyPageContainer",
                                Cls = "pageContainer",
                                Width = 1120,
                                Height = 1300,
                            },
                            new Panel
                            {

                            },
                        }
                    },
                }
            };
        }
    }
}