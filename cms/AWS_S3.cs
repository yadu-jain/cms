using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

/// <summary>
/// Summary description for AWS_S3
/// </summary>

public class AWS_S3
{
    string strBucketName = null;
    string strDefaultFolder = null;
    string strS3Url = null;
    TransferUtility transferUtility = null;

    public AWS_S3()
    {
        loadConfiguration();
    }
    public AWS_S3(string strBucketName)
    {
        loadConfiguration();
        this.strBucketName = strBucketName;
    }
    public bool uploadFile(string filePath, string strKeyName)
    {
        strKeyName = strDefaultFolder + "/" + strKeyName;
        try
        {
            this.transferUtility.Upload(filePath, this.strBucketName, strKeyName);
            return true;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Please check the provided AWS Credentials.");
                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
            }
            else
            {
                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
            }
            return false;
        }


    }
    public bool uploadFile(Stream fileStream, string strKeyName)
    {
        strKeyName = strDefaultFolder + "/" + strKeyName;
        try
        {
            this.transferUtility.Upload(fileStream, this.strBucketName, strKeyName);
            return true;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Please check the provided AWS Credentials.");
                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
            }
            else
            {
                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
            }
            return false;
        }
    }
    public bool uploadFileAdvacned(string strFilePath, string strKeyName, Dictionary<string, string> dict)
    {
        strKeyName = strDefaultFolder + "/" + strKeyName;
        try
        {
            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = this.strBucketName,
                FilePath = strFilePath,
                //StorageClass = S3StorageClass.ReducedRedundancy,
                //PartSize = 6291456, // 6 MB.
                Key = strKeyName,
                CannedACL = S3CannedACL.PublicRead
            };
            foreach (String key in dict.Keys)
            {
                fileTransferUtilityRequest.Metadata.Add(key, dict[key]);
            }
            this.transferUtility.Upload(fileTransferUtilityRequest);
            return true;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Please check the provided AWS Credentials.");
                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
            }
            else
            {
                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
            }
            return false;
        }
    }
    public string uploadFileAdvacned(Stream fileStream, string strKeyName, Dictionary<string, string> dict)
    {
        strKeyName = this.strDefaultFolder + "/" + strKeyName;
        try
        {

            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = this.strBucketName,
                InputStream = fileStream,
                //StorageClass = S3StorageClass.ReducedRedundancy,
                //PartSize = 6291456, // 6 MB.
                Key = strKeyName,
                CannedACL = S3CannedACL.PublicRead
            };
            foreach (String key in dict.Keys)
            {
                fileTransferUtilityRequest.Metadata.Add(key, dict[key]);
            }
            this.transferUtility.Upload(fileTransferUtilityRequest);
            string strLink = this.strS3Url + this.strBucketName + "/" + strKeyName;
            return strLink;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Please check the provided AWS Credentials.");
                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
            }
            else
            {
                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
            }
            return "";
        }
    }
    public string uploadFile(Stream fileStream, string strBucketName, string strFolder, string strFileName, Dictionary<string, string> dict)
    {
        string strLink = strFolder + "/" + strFileName;
        try
        {

            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = strBucketName,
                InputStream = fileStream,
                //StorageClass = S3StorageClass.ReducedRedundancy,
                //PartSize = 6291456, // 6 MB.
                Key = strLink,
                CannedACL = S3CannedACL.PublicRead
            };
            foreach (String key in dict.Keys)
            {
                fileTransferUtilityRequest.Metadata.Add(key, dict[key]);
            }
            this.transferUtility.Upload(fileTransferUtilityRequest);
            strLink = this.strS3Url + strBucketName + "/" + strLink;
            return strLink;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Please check the provided AWS Credentials.");
                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
            }
            else
            {
                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
            }
            return "";
        }
    }


    private void loadConfiguration()
    {
        NameValueCollection appConfig = ConfigurationSettings.AppSettings;

        if (string.IsNullOrEmpty(appConfig["AWS_AccessKey"]))
        {
            throw new Exception("AWS Access key not found in config !");
        }

        if (string.IsNullOrEmpty(appConfig["AWS_SecretKey"]))
        {
            throw new Exception("AWS Secret key not found in config !");
        }

        this.transferUtility = new TransferUtility(appConfig["AWS_AccessKey"], appConfig["AWS_SecretKey"], RegionEndpoint.APSoutheast1); //Singapore            
        // Update the Bucket to the optionally supplied Bucket from the App.config.
        this.strBucketName = appConfig["AWS_DefaultBucket"];
        this.strDefaultFolder = appConfig["AWS_DefaultFolder"];
        this.strS3Url = appConfig["AWS_S3_URL"];
    }
}