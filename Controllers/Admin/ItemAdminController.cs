using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Models.Items;
using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AspShop.Helpers.Params;

namespace AspShop.Controllers.Admin
{
    [Authorize(Roles = "Administrators")]
    public class ItemAdminController : Controller
    {
        public AppDbContext db = new AppDbContext();
        public FieldManager fieldManager = new FieldManager();
        public string addTo = "rightContainer";
        public ActionResult GetAll(StoreRequestParameters parameters)
        {
            int limit = parameters.Limit;
            int start = parameters.Start;

            var items = db.Items.ToList();

            if ((start + limit) > items.Count)
            {
                limit = items.Count - start;
            }

            var rangeItems = (start < 0 || limit < 0) ? items : items.GetRange(start, limit);

            return this.Store(new Paging<Item>(rangeItems, items.Count));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetById(Guid id)
        {
            var item = db.Items.First(ii => ii.Id == id);

            var panel = fieldManager.RenderFields(new FieldCreateParams
            {
                ModelsForParse = new List<object> { item, item.Laptop, item.Smartphone },
                ButtonUrlAction = Url.Action("Edit", "ItemAdmin"),
                ButtonText = "Редактировать товар"
            });

            this.GetCmp<Panel>(addTo).RemoveAll();
            panel.AddTo(this.GetCmp<Panel>(addTo));

            return this.Direct();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formcol)
        {
            var list = fieldManager.EditModel(new FieldEditParams
            {
                ModelsForEdit = new List<Type> { typeof(Item), typeof(Laptop), typeof(Smartphone) },
                InputCollection = formcol
            });

            return RedirectToAction("Index", "Admin");
        }
    }
}
