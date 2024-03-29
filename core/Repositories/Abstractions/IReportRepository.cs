﻿using api.core.data.entities;

namespace api.core.repositories.abstractions;

public interface IReportRepository: IRepository<Report>
{
    public IEnumerable<Report> GetRecentReports(int lastSeconds);
}
