#region license
// MIT License
// 
// Copyright (c) [2017] [Tobias Ruby]
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Bpm.Helpers;
using BpmApi.Models;

namespace BpmApi.Controllers
{
    [RoutePrefix("api/activityattributes")]
    public class ActivityAttributeController : ApiController
    {
        private readonly DataHelper.ActivityAttributeHelper _instance = DataHelper.ActivityAttributeHelper.Instance();

        [Route("")]
        [HttpGet]
        public ActivityAttributeModel[] GetActivityAttributes()
        {
            return _instance.Models.ToArray();
        }

        [Route("")]
        [HttpPost]
        public ActivityAttributeModel CreateActivityAttribute([FromBody] ActivityAttributeModel model)
        {
            _instance.Models.Add(model);
            return model;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public ActivityAttributeModel GetActivityAttribute(string id)
        {
            return _instance.Models.Single(x => x.id.Equals(id));
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivityAttribute(Guid id)
        {
            var model = _instance.Models.Single(x => x.id.Equals(id));
            var success = _instance.Models.Remove(model);

            if (success)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.OK;
        }
    }
}