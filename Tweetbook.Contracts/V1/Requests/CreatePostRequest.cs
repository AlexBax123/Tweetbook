﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public String Name { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
