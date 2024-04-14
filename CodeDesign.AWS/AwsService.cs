using Amazon.Runtime;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.AWS
{
    /// <summary>
    /// <see cref="https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/dotnetv3/S3"/>
    /// </summary>
    public abstract class AwsServiceBase
    {
        protected static BasicAWSCredentials _credential = new BasicAWSCredentials("", "");
    }
}
