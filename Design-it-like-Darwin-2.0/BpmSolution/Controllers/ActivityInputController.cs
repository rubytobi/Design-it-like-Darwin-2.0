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
using MoreLinq;

namespace WebApi.Controllers
{
    [RoutePrefix("api/activityinputs")]
    public class ActivityInputController : ApiController
    {
        private readonly DataHelper.ActivityInputHelper _instance = DataHelper.ActivityInputHelper.Instance();

        [Route("")]
        [HttpPost]
        public void CreateActivityInputs([FromBody] InputModel[] inputs)
        {
            inputs.ForEach(x => _instance.Models.Add(x));
        }

        [Route("")]
        [HttpGet]
        public InputModel[] GetActivityInputs()
        {
            return _instance.Models.ToArray();
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public HttpStatusCode DeleteActivityInputs([FromUri] Guid id)
        {
            var single = _instance.Models.Single(x => x.id.Equals(id));

            if (single == null)
                return HttpStatusCode.BadRequest;

            _instance.Models.Remove(single);
            return HttpStatusCode.OK;
        }
    }
}