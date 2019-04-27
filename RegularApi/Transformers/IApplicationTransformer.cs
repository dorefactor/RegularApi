﻿using RegularApi.Domain.Views;
using RegularApi.Domain.Model;

namespace RegularApi.Transformers
{
    public interface IApplicationTransformer
    {

        Application fromResource(ApplicationView applicationView);
    }
}
