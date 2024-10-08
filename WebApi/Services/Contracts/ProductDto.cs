﻿using WebApi.Shared;

namespace WebApi.Services.Contracts;

public record ProductDto
(
    Guid? Id,
    string Name,
    string Description,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);